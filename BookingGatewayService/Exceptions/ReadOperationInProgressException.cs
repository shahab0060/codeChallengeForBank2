using System;

namespace BookingGatewayService.Exceptions
{
    public sealed class ReadOperationInProgressException : Exception
    {
        #region constructor

        public ReadOperationInProgressException() : base("Failed because there is a read operation which is in progress!")
        {

        }

        #endregion
    }
}
