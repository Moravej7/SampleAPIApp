using System;

namespace Common.Exceptions
{
    public class BadRequestException : AppException
    {
        public BadRequestException()
            : base()
        {
        }

        public BadRequestException(string message)
            : base(message, System.Net.HttpStatusCode.BadRequest)
        {
        }

        public BadRequestException(object additionalData)
            : base(additionalData)
        {
        }

        public BadRequestException(string message, object additionalData)
            : base(message, additionalData)
        {
        }

        public BadRequestException(string message, Exception exception)
            : base(message, exception)
        {
        }

        public BadRequestException(string message, Exception exception, object additionalData)
            : base(message, exception, additionalData)
        {
        }
    }
}
