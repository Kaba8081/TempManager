using TempManager.Shared.Interfaces;

namespace TempManager.Shared.Utilities
{
    public static class Log
    {
        private static ILogger _logger;

        public static void Initialize(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public static void Info(string message) => _logger?.Info(message);
        public static void Warn(string message) => _logger?.Warn(message);
        public static void Error(string message, Exception ex = null) => _logger?.Error(message, ex);
    }
}
