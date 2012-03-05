﻿using System;

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

        public DbResultInfoRtn(string errorMsg, string innerException, T value)
            : base(errorMsg, innerException)
        {
            Value = value;
        }

        public DbResultInfoRtn(string errorMsg, string innerException, object value)
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

        public DbResultInfo(string errorMsg, string innerErrorMessage)
        {
            InnerErrorMessage = innerErrorMessage;
            Success = !(HasErrorBeenSet(errorMsg) || HasErrorBeenSet(innerErrorMessage));
            ErrorMessage = errorMsg;
        }

        public bool Success { get; private set; }
        public string ErrorMessage { get; private set; }
        public string InnerErrorMessage { get; private set; }

        public Exception AsException
        {
            get
            {
                return
                    new Exception(ErrorMessage +
                                  (string.IsNullOrEmpty(InnerErrorMessage)
                                       ? string.Empty
                                       : Environment.NewLine + InnerErrorMessage));
            }
        }

        private bool HasErrorBeenSet(string errorMsg)
        {
            return !string.IsNullOrEmpty(errorMsg);
        }
    }
}