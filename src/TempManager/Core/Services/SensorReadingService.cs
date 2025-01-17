using TempManager.Core.Interfaces;
using Domain.Models;
using Domain.Services.Interfaces;
using Logger.Utilities;

namespace TempManager.Core.Services
{
    public class SensorReadingService : ISensorReadingService
    {
        private Dictionary<TMSensor, SensorReading> _readings;
        private IFileHandler _fileHandler;

        public SensorReadingService() 
        {
            _readings = new Dictionary<TMSensor, SensorReading>();
        }
        public SensorReadingService(IFileHandler fileHandler) : this()
        {
            _fileHandler = fileHandler;
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

        public void UpdateTrackedSensors() 
        {
            foreach (var sensor in _readings.Keys)
                _readings[sensor].Data.Add((float)(sensor.Value ?? 0.0));

            return;
        }

        public Task SaveTrackedSensors() 
        {
            List<SensorReading> data = _readings.Values.ToList();
            
            _fileHandler?.Save(data, "saved_results");

            return Task.CompletedTask;
        }
    }
}
