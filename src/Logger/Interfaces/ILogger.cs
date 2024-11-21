namespace Logger.Interfaces
{
    public interface ICustomLogger
    {
        void Info(string message);
        void Warn(string message);
        void Error(string message, Exception exception = null);
    }
}
