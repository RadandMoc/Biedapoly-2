using System;
using System.Runtime.Serialization;

namespace Biedapoly
{
    [Serializable]
    internal class WrongUsingOfFunctionExcepting : Exception
    {
        public WrongUsingOfFunctionExcepting()
        {
        }

        public WrongUsingOfFunctionExcepting(string message) : base(message)
        {
        }

        public WrongUsingOfFunctionExcepting(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WrongUsingOfFunctionExcepting(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}