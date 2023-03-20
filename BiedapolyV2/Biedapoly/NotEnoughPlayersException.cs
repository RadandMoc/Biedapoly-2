using System;
using System.Runtime.Serialization;

namespace Biedapoly
{
    [Serializable]
    internal class NotEnoughPlayersException : Exception
    {
        public NotEnoughPlayersException()
        {
        }

        public NotEnoughPlayersException(string message) : base(message)
        {
        }

        public NotEnoughPlayersException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotEnoughPlayersException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}