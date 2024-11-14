using LibreHardwareMonitor.Hardware;

namespace TempManager.Shared.Models
{
    public class TMSensor
    {
        private ISensor _sensor;
        public string Name { get; set; }
        public SensorType SensorType { get; set; }
        public double? Value { get; set; }
        public string HardwareName { get; set; }

        public TMSensor(ISensor sensor)
        {
            _sensor = sensor;
            Name = sensor.Name;
            SensorType = sensor.SensorType;
            Value = sensor.Value;
            HardwareName = sensor.Hardware.Name;
        }
        public void Update()
        {
            Value = _sensor.Value;
        }

    }
}
