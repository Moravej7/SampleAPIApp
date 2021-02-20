using System;

namespace Common.Exceptions
{
    public class LogicException : AppException
    {
        public LogicException() 
            : base()
        {
        }

        public LogicException(string message) 
            : base(message)
        {
        }

        public LogicException(object additionalData) 
            : base(additionalData)
        {
        }

        public LogicException(string message, object additionalData) 
            : base(message, additionalData)
        {
        }

        public LogicException(string message, Exception exception)
            : base(message, exception)
        {
        }

        public LogicException(string message, Exception exception, object additionalData)
            : base(message, exception, additionalData)
        {
        }
    }
}
