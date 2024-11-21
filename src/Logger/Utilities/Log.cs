using Logger.Interfaces;

namespace Logger.Utilities
{
    public static class Log
    {
        private static ICustomLogger _logger;

        public static void Initialize(ICustomLogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public static void Info(string message) => _logger?.Info(message);
        public static void Warn(string message) => _logger?.Warn(message);
        public static void Error(string message, Exception ex = null) => _logger?.Error(message, ex);
    }
}
