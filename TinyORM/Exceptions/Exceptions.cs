using System;

namespace TinyORM.Exceptions
{
    public class TinyMapperException : Exception
    {
        public TinyMapperException() { }
        public TinyMapperException(string message) : base(message) { }
        public TinyMapperException(string message, Exception innerException) : base(message, innerException) { }

        protected TinyMapperException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
    public class TinyDbException : Exception
    {
        public TinyDbException() { }
        public TinyDbException(string message) : base(message) { }
        public TinyDbException(string message, Exception innerException) : base(message, innerException) { }

        protected TinyDbException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}