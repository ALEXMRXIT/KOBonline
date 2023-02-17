using System;

#pragma warning disable CS8632 // nullable

namespace Assets.Sources.Models
{
    public struct PacketImplementCodeResult
    {
        public PacketImplementCodeResult(int errorCode, string message,
            string fileException, Exception? exception = default)
        {
            ErrorCode = errorCode;
            ErrorMessage = message;
            InnerException = exception;
            FireException = fileException;
        }

        public int ErrorCode;
        public string ErrorMessage;
        public Exception? InnerException;
        public string FireException;
    }
}