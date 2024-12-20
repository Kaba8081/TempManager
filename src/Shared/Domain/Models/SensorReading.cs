
namespace Domain.Models
{
    public class SensorReading
    {
        public string Name { get; set; } = "invalid";
        public TMSensorType Type { get; set; }
        public List<float> Data { get; set; } = new List<float> { };
    }
}
