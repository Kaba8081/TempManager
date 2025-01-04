
namespace Domain.Models
{
    public class SensorReading
    {
        public SensorReading(TMSensor sensor)
        {
            Name = sensor.Name;
            Type = sensor.SensorType;
        }

        public string Name { get; set; } = "invalid";
        public TMSensorType Type { get; set; }
        public List<float> Data { get; set; } = new List<float> { };
    }
}
