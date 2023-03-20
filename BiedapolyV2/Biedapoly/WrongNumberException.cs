using System;
using System.Runtime.Serialization;

namespace Biedapoly
{
    [Serializable]
    internal class WrongNumberException : Exception
    {
        public WrongNumberException()
        {
        }

        public WrongNumberException(string message) : base(message)
        {
        }

        public WrongNumberException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WrongNumberException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}