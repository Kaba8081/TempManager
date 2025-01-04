using Notifier.Interfaces;
using Logger.Utilities;

namespace Notifier.Utilities
{
    public static class Notify
    {
        private static INotifier _notifier;

        public static void Initialize(INotifier notifier)
        {
            _notifier = notifier ?? throw new ArgumentNullException(nameof(notifier));
        }
        public static void RegisterEvent(string eventName, Delegate callback)
        {
            Log.Debug($"Registering event: {eventName}");
            _notifier?.RegisterEvent(eventName, callback);
        }
        public static void UnRegisterEvent(string eventName, Delegate callback)
        {
            Log.Debug($"Unregister event: {eventName}");
            _notifier?.UnregisterEvent(eventName, callback);
        }
        public static void TriggerEvent(string eventName, params object[]? args)
        {
            Log.Debug($"{eventName} triggered with parameters: {args}");
            _notifier?.TriggerEvent(eventName, args);
        }
    }
}
