using BookingGatewayRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookingGatewayService
{
    /// <summary>
    /// TODO: The class should be protected from inheritance!
    /// </summary>
    public sealed class BookingGatewayFactory
    {
        /// <summary>
        /// TOOD: The method should create instance of IBookingGateway
        /// </summary>
        /// <param name="repository"></param>
        /// <returns></returns>
        public static IBookingGateway CreateGateway(IDBRepository repository)
        {
            return new BookingGateway(repository);
        }

        /// <summary>
        /// TODO: The method should be deprecated, but developer can use it!
        /// </summary>
        /// <returns></returns>
        [Obsolete("Use CreateGateway instead.")]
        public static object CreateObject()
        {
            return new BookingGatewayFactory();
        }

        /// <summary>
        /// TODO: The method should be deprecated! Developer cannot use it! If use it there should be compilation error!
        /// </summary>
        /// <returns></returns>
        [Obsolete("NewObject is deprecated and must not be used.", true)]
        public static object NewObject()
        {
            return new object();
        }
    }
}
