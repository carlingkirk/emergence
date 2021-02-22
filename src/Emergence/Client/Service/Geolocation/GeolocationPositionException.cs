using System;
using System.Runtime.Serialization;

namespace Emergence.Service.Geolocation
{
    /// <summary>
    /// Similar to the <see cref="GeolocationPositionError"/> class, but used as an exception
    /// </summary>
    public class GeolocationPositionException : Exception
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="GeolocationPositionException"/> class from a <see cref="GeolocationPositionError"/> object
        /// </summary>
        /// <param name="error"></param>
        public GeolocationPositionException(GeolocationPositionError error) : base(error.Message)
        {
            Code = error.Code;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="GeolocationPositionException"/> class
        /// </summary>
        public GeolocationPositionException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeolocationPositionException"/> class
        /// </summary>
        /// <param name="message">The exception message</param>
        public GeolocationPositionException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeolocationPositionException"/> class
        /// </summary>
        /// <param name="message">The exception message</param>
        /// <param name="innerException">The inner exception</param>
        public GeolocationPositionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeolocationPositionException"/> class
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> object that holds the serialized object data about the exception being thrown </param>
        /// <param name="context">The <see cref="StreamingContext"/> that holds contextual information about the source or destination</param>
        protected GeolocationPositionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <inheritdoc cref="GeolocationPositionError.Code"/>
        public ushort Code { get; set; }

    }
}
