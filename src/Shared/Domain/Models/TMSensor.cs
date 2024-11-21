using Shared.Domain.Utilities;
using LibreHardwareMonitor.Hardware;

namespace Shared.Domain.Models
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
