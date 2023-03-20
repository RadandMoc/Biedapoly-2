using System;
using System.Runtime.Serialization;

namespace Biedapoly
{
    [Serializable]
    internal class WrongBonusException : Exception
    {
        public WrongBonusException()
        {
        }

        public WrongBonusException(string message) : base(message)
        {
        }

        public WrongBonusException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WrongBonusException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}