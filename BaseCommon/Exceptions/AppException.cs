using System;
using System.Net;

namespace Common.Exceptions
{
    public class AppException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public object AdditionalData { get; set; }

        public AppException()
        {
        }

        public AppException(string message)
            : this(message, HttpStatusCode.InternalServerError)
        {
        }


        public AppException(object additionalData)
            : this(null, additionalData)
        {
        }

        public AppException(string message, object additionalData)
            : this( message, HttpStatusCode.InternalServerError, additionalData)
        {
        }

        public AppException(string message, HttpStatusCode httpStatusCode)
            : this( message, httpStatusCode, null)
        {
        }

        public AppException(string message, HttpStatusCode httpStatusCode, object additionalData)
            : this(message, httpStatusCode, null, additionalData)
        {
        }

        public AppException(string message, Exception exception)
            : this(message, HttpStatusCode.InternalServerError, exception)
        {
        }

        public AppException(string message, Exception exception, object additionalData)
            : this(message, HttpStatusCode.InternalServerError, exception, additionalData)
        {
        }

        public AppException(string message, HttpStatusCode httpStatusCode, Exception exception)
            : this(message, httpStatusCode, exception, null)
        {
        }

        public AppException(string message, HttpStatusCode httpStatusCode, Exception exception, object additionalData)
            : base(message, exception)
        {
            HttpStatusCode = httpStatusCode;
            AdditionalData = additionalData;
        }
    }
}
