using System;

namespace BookingGatewayService.Exceptions
{
    public sealed class NoStartBookingException : Exception
    {
        #region constructor

        public NoStartBookingException() : base("Failed because no booking has been started before this")
        {
            
        }

        #endregion
    }
}
