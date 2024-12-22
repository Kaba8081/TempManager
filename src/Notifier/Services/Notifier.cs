using Notifier.Interfaces;
using Logger.Utilities;

namespace Notifier.Services
{
    public class CustomNotifier : INotifier
    {
        private readonly Dictionary<string, Delegate> _events = new();

        public void RegisterEvent(string eventName, Delegate callback) 
        {
            if (_events.ContainsKey(eventName))
            {
                Log.Warn($"There is already an event with id: {eventName}");
                return;
            }

            _events[eventName] = callback;
        }
        public void UnregisterEvent(string eventName, Delegate callback) 
        {
            if (_events.ContainsKey(eventName)) _events[eventName] = null;
        }
        public void TriggerEvent(string eventName, params object[] ?args)
        {
            if (!_events.ContainsKey(eventName) || _events[eventName] == null)
            {
                Log.Error($"No event wih id: {eventName}");
            }

            else if (_events[eventName] is Action && args == null)
            {
                ((Action)_events[eventName]).Invoke();
            }
            else if (_events[eventName] is Action<object[]> ActionWithArguments)
            {
                if (args == null) Log.Warn($"{eventName} event missing required arguments!");
                else ActionWithArguments.Invoke(args); 
            }
            else
            {
                Log.Error($"Uknown error occured trying to invoke: {eventName}");
            }
        }
    }
}
