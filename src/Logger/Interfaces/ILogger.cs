﻿namespace Logger.Interfaces
{
    public interface ICustomLogger
    {
        void Debug(string message);
        void Info(string message);
        void Warn(string message);
        void Error(string message, Exception exception = null);
    }
}
