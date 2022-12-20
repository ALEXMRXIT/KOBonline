using System;

#pragma warning disable CS8632 // nullable

namespace Assets.Sources.Models
{
    public readonly struct PacketImplementCodeResult
    {
        public PacketImplementCodeResult(int errorCode, string message, Exception? exception = default)
        {
            ErrorCode = errorCode;
            ErrorMessage = message;
            InnerException = exception;
        }

        public readonly int ErrorCode;
        public readonly string ErrorMessage;
        public readonly Exception? InnerException;
    }
}