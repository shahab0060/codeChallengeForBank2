using System;

namespace BookingGatewayService.Exceptions
{
    public sealed class BookingInProgressException : Exception
    {
        #region constructor

        public BookingInProgressException() : base("failed because a booking is already in progress.")
        {
            
        }

        #endregion
    }
}
