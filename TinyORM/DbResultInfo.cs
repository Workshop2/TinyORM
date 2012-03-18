using System;
using TinyORM.Exceptions;

namespace TinyORM
{
    public class DbResultInfoRtn<T>
        : DbResultInfo
    {
        public DbResultInfoRtn(string errorMsg, T value)
            : base(errorMsg)
        {
            Value = value;
        }

        public DbResultInfoRtn(string errorMsg, object value)
            : base(errorMsg)
        {
            if (value != null)
                Value = (T) value;
        }

        public DbResultInfoRtn(string errorMsg, Exception innerException, T value)
            : base(errorMsg, innerException)
        {
            Value = value;
        }

        public DbResultInfoRtn(string errorMsg, Exception innerException, object value)
            : base(errorMsg, innerException)
        {
            if (value != null)
                Value = (T) value;
        }

        public T Value { get; set; }
    }

    public class DbResultInfo
    {
        public DbResultInfo()
        {
            Success = true;
        }

        public DbResultInfo(string errorMsg)
        {
            Success = !HasErrorBeenSet(errorMsg);
            ErrorMessage = errorMsg;
        }

        public DbResultInfo(string errorMsg, Exception innerErrorMessage)
        {
            InnerError = innerErrorMessage;
            Success = !(HasErrorBeenSet(errorMsg) || HasErrorBeenSet(innerErrorMessage));
            ErrorMessage = errorMsg;
        }

        public bool Success { get; private set; }
        public string ErrorMessage { get; private set; }
        public Exception InnerError { get; private set; }

        public TinyDbException AsException
        {
            get
            {
                return new TinyDbException(ErrorMessage, InnerError);
            }
        }

        private static bool HasErrorBeenSet(string errorMsg)
        {
            return !string.IsNullOrEmpty(errorMsg);
        }

        private static bool HasErrorBeenSet(Exception error)
        {
            return error != null;
        }
    }
}