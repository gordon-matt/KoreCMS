using System;
using System.Runtime.Serialization;

namespace Kore.Exceptions
{
    public class KoreException : Exception
    {
        public KoreException()
        {
        }

        public KoreException(string message)
            : base(message)
        {
        }

        public KoreException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected KoreException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}