using BookingGatewayRepository;
using BookingGatewayRepository.Model;
using BookingGatewayService.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BookingGatewayService
{
    /// <summary>
    /// The class that implements IBookingGateway and is sealed to prevent inheritance.
    /// </summary>
    public sealed class BookingGateway : IBookingGateway
    {
        // Shared concurrency control (applies across all instances)
        private static readonly object _lock = new object();
        private static int? _bookingThreadId = null;
        private static int _activeStatusReaders = 0;

        // Per-instance flag to track if StartBooking was called
        private bool _hasStartedBooking = false;

        public IDBRepository DBRepository { get; set; }

        public BookingGateway(IDBRepository repository)
        {
            DBRepository = repository;
        }

        public void StartBooking()
        {
            int currentThreadId = Thread.CurrentThread.ManagedThreadId;

            lock (_lock)
            {
                if (_bookingThreadId != null)
                    throw new BookingInProgressException();

                if (_activeStatusReaders > 0)
                    throw new ReadOperationInProgressException();

                _bookingThreadId = currentThreadId;
                _hasStartedBooking = true;
            }
        }

        public void Booking(string uniqueReference, decimal amount, string transactionTitle, string srcAccountNo, string destAccountNo)
        {
            if (!_hasStartedBooking)
                throw new NoStartBookingException();

            var data = new TransactionData
            {
                UniqueRef = uniqueReference,
                Amount = amount,
                TransactionTitle = transactionTitle,
                SourceAccountNo = srcAccountNo,
                DestAccountNo = destAccountNo
            };

            DBRepository.SaveBooking(data);
        }

        public void EndBooking()
        {
            int currentThreadId = Thread.CurrentThread.ManagedThreadId;

            if (!_hasStartedBooking)
                throw new NoStartBookingException();

            lock (_lock)
            {
                if (_bookingThreadId == currentThreadId)
                {
                    _bookingThreadId = null;
                    _hasStartedBooking = false;
                }
            }
        }

        public IList<TransactionStatus> GetBookingStatuses(IList<string> uniqueTransactionRefs)
        {
            int currentThreadId = Thread.CurrentThread.ManagedThreadId;

            lock (_lock)
            {
                if (_bookingThreadId != null && _bookingThreadId != currentThreadId)
                    throw new BookingInProgressException();

                _activeStatusReaders++;
            }

            try
            {
                if (uniqueTransactionRefs == null || uniqueTransactionRefs.Count == 0)
                    return new List<TransactionStatus>();

                var result = DBRepository.GetStatuses(uniqueTransactionRefs.ToArray());
                return result != null ? new List<TransactionStatus>(result) : new List<TransactionStatus>();
            }
            finally
            {
                lock (_lock)
                {
                    _activeStatusReaders--;
                }
            }
        }
    }
}
