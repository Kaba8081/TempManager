namespace Domain.DTO
{
    internal class SensorReadingDTO
    {
        public string Name { get; set; } = "invalid";
        public string Type { get; set; } = "invalid";
        public List<float> Data { get; set; } = new List<float> { };
    }
}
