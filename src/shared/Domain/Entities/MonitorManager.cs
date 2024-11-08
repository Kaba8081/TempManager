namespace Domain.Entities
{
    using LibreHardwareMonitor.Hardware;

    public class UpdateVisitor : IVisitor
    {
        public void VisitComputer(IComputer computer)
        {
            computer.Traverse(this);
        }
        public void VisitHardware(IHardware hardware)
        {
            hardware.Update();
            foreach (IHardware subHardware in hardware.SubHardware) subHardware.Accept(this);
        }
        public void VisitSensor(ISensor sensor) { }
        public void VisitParameter(IParameter parameter) { }
    }
    public class MonitorManager
    {
        private readonly Computer _computer;
        private IList<IHardware> _activeHardware = new List<IHardware>();

        public MonitorManager()
        {
            _computer = new Computer
            {
                IsCpuEnabled = true,
                IsGpuEnabled = true,
                IsMemoryEnabled = true,
                IsMotherboardEnabled = true,
                IsControllerEnabled = true,
                IsNetworkEnabled = true,
                IsStorageEnabled = true
            };

            lock (_computer)
            {
                _computer.Open();
                _computer.Accept(new UpdateVisitor());
                _activeHardware = _computer.Hardware;
            }
        }
        public MonitorManager(ISettings settings)
        {
            _computer = new Computer(settings);

            _computer.Open();
            _computer.Accept(new UpdateVisitor());

            _activeHardware = _computer.Hardware;

        }
        ~MonitorManager()
        {
            _computer.Close();
        }

        public async Task Update()
        {
            foreach (var hw in _activeHardware) hw.Update();

            await Task.CompletedTask;
        }

        public Dictionary<SensorType, List<ISensor>> GetSensors(IHardware hardware)
        {
            var sensor_dict = new Dictionary<SensorType, List<ISensor>>();

            foreach (var key in Enum.GetValues(typeof(SensorType)))
            {
                var sensors = hardware.Sensors.Where(s => s.SensorType == (SensorType)key);
                sensor_dict.Add((SensorType)key, sensors.ToList());
            }

            return sensor_dict;
        }
        public IList<IHardware> GetHardware()
        {
            return _activeHardware;
        }
        public void Monitor()
        {

            foreach (IHardware hardware in _computer.Hardware)
            {
                Console.WriteLine("Hardware: {0}", hardware.Name);

                foreach (IHardware subhardware in hardware.SubHardware)
                {
                    Console.WriteLine("\tSubhardware: {0}", subhardware.Name);

                    foreach (ISensor sensor in subhardware.Sensors)
                    {
                        Console.WriteLine("\t\tSensor: {0}, value: {1}", sensor.Name, sensor.Value);
                    }
                }

                var sensors = hardware.Sensors.OrderBy(s => s.SensorType).ToList();
                foreach (ISensor sensor in sensors)
                {
                    Console.WriteLine("\tSensor: {0}, type: {1}, value: {2}", sensor.Name, sensor.SensorType, sensor.Value);
                }
            }
        }
    }
}
