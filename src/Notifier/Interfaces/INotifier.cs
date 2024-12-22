namespace Notifier.Interfaces
{
    public interface INotifier
    {
        void RegisterEvent(string eventName, Delegate callback);
        void UnregisterEvent(string eventName, Delegate callback);
        void TriggerEvent(string eventName, params object[]? args);
    }
}
