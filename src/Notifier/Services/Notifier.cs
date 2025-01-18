using Notifier.Interfaces;
using Logger.Utilities;

namespace Notifier.Services
{
    public class CustomNotifier : INotifier
    {
        private readonly Dictionary<string, Delegate> _events = new();
        private List<string> _crashed_events = new();

        private void ExecuteDelegate(string eventName, params object[]? args) 
        {
            var _eventDelegate = _events[eventName];

            if (_eventDelegate is Action action && (args == null || args.Length == 0))
            {
                action.Invoke();
            }
            else if (_eventDelegate is Delegate del)
            {
                var paramTypes = del.Method.GetParameters().Select(p => p.ParameterType).ToArray();

                if (paramTypes.Length == args?.Length)
                    del.DynamicInvoke(args);
                else
                    Log.Error($"Invalid number of arguments for event: {eventName}");
            }
            else
            {
                Log.Error($"Unsupported delegate type for event: {eventName}. Expected Action or Action<T>.");
            }

            return;
        }

        public void RegisterEvent(string eventName, Delegate callback) 
        {
            if (_events.ContainsKey(eventName))
            {
                Log.Warn($"There is already an event with id: {eventName}");
                return;
            }

            _events[eventName] = callback;
            return;
        }
        public void UnregisterEvent(string eventName, Delegate callback) 
        {
            if (_events.ContainsKey(eventName)) 
                _events[eventName] = null;
            
            return;
        }
        public void TriggerEvent(string eventName, params object[]? args)
        {
            if (_crashed_events.Contains(eventName))
                return;

            if (!_events.ContainsKey(eventName) || _events[eventName] == null)
            {
                _crashed_events.Append(eventName);
                Log.Error($"No event wih id: {eventName}");
                return;
            }

            try {
                ExecuteDelegate(eventName, args);
            } 
            catch (Exception ex)
            {
                _crashed_events.Append(eventName);
                Log.Error($"Error invoking event: {eventName} with args: {args} - {ex.Message}");
                return;
            }

            return;
        }
    }
}
