using TempManager.Core.Interfaces;
using Domain.Models;
using Logger.Utilities;

namespace TempManager.Core.Services
{
    public class SensorReadingService : ISensorReadingService
    {
        private Dictionary<TMSensor, SensorReading> _readings;

        public SensorReadingService() 
        {
            _readings = new Dictionary<TMSensor, SensorReading>();
        }

        private void TrackSensor(TMSensor sensor) 
        {
            Log.Debug($"Tracking sensor: {sensor.Name}");
            
            if (!_readings.ContainsKey(sensor))
                _readings.Add(sensor, new SensorReading(sensor));

            return;
        }
        private void UntrackSensor(TMSensor sensor) 
        {
            if (!_readings.ContainsKey(sensor)) return;

            Log.Debug($"Untracking sensor: {sensor.Name}");
            _readings.Remove(sensor);

            return;
        }

        public void CheckTrackedSensors(IList<TMSensor> new_sensors)
        {
            foreach (var sensor in new_sensors)
            {
                if (!_readings.ContainsKey(sensor))
                    TrackSensor(sensor);
            }

            foreach (var sensor in _readings.Keys)
            {
                if (!new_sensors.Contains(sensor))
                    UntrackSensor(sensor);
            }

            return;
        }

        public void UpdateTrackedSensors(IList<TMSensor> sensors) 
        {
            // TODO: Implement this method
            return;
        }
    }
}
