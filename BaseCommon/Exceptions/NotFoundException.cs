using System;

namespace Common.Exceptions
{
    public class NotFoundException : AppException
    {
        public NotFoundException()
            : base()
        {
        }

        public NotFoundException(string message)
            : base(message)
        {
        }

        public NotFoundException(object additionalData)
            : base(additionalData)
        {
        }

        public NotFoundException(string message, object additionalData)
            : base(message, additionalData)
        {
        }

        public NotFoundException(string message, Exception exception)
            : base(message, exception)
        {
        }

        public NotFoundException(string message, Exception exception, object additionalData)
            : base(message, exception, additionalData)
        {
        }
    }
}
