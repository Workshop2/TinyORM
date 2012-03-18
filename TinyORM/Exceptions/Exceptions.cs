using System;

namespace TinyORM.Exceptions
{
    public class TinyMapperException : Exception
    {
        public TinyMapperException() { }
        public TinyMapperException(string message){}
        public TinyMapperException(string message, Exception inner) { }

        protected TinyMapperException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) { }
    }
    public class TinyDbException : Exception {
        public TinyDbException() { }
        public TinyDbException(string message){}
        public TinyDbException(string message, Exception inner) { }

        protected TinyDbException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) { }
    }
}