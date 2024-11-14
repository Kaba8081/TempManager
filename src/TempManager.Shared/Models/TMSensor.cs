using LibreHardwareMonitor.Hardware;
using TempManager.Shared.Utilities;

namespace TempManager.Shared.Models
{
    public class TMSensor
    {
        private ISensor _sensor;
        public string Name { get; set; }
        public TMSensorType SensorType { get; set; }
        public double? Value { get; set; }
        public string HardwareName { get; set; }

        public TMSensor(ISensor sensor)
        {
            _sensor = sensor;
            Name = sensor.Name;
            SensorType = SensorTypeMapper.ToTMSensorType(sensor.SensorType);
            Value = sensor.Value;
            HardwareName = sensor.Hardware.Name;
        }
        public void Update()
        {
            Value = _sensor.Value;
        }

    }
}
