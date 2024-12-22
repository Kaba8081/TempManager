using LibreHardwareMonitor.Hardware;
using TempManager.Core.Interfaces;
using Domain.Models;

namespace TempManager.Core.Services
{
    public class HardwareService : IHardwareService
    {
        private readonly Computer _computer;
        private IList<TMHardware> _activeHardware = new List<TMHardware>();
        public IList<SensorReading> trackedSensors = new List<SensorReading>();

        public HardwareService()
        {
            _computer = new Computer
            {
                IsCpuEnabled = true,
                IsGpuEnabled = true,
                IsMotherboardEnabled = true,
                IsMemoryEnabled = true,
                IsStorageEnabled = true,
                IsNetworkEnabled = true
            };

            _computer.Open();
            _activeHardware = _computer.Hardware.Select(hardware => new TMHardware(hardware)).ToList();
        }
        ~HardwareService()
        {
            _computer.Close();
        }
        public async Task Update()
        {
            foreach (var hardware in _activeHardware) hardware.Update();
            await Task.CompletedTask;
        }

        public IList<TMHardware> GetHardwareComponents()
        {
            if (_activeHardware.Count > 0)
            {
                return _activeHardware;
            }

            var hardwareList = new List<TMHardware>();

            foreach (var hardware in _computer.Hardware)
            {
                hardwareList.Add(new TMHardware(hardware));
            }

            return hardwareList;
        }

        public Dictionary<TMSensorType, List<TMSensor>> GetGroupedSensors(TMHardware hardware)
        {
            var sensorDictionary = new Dictionary<TMSensorType, List<TMSensor>>();

            foreach (var key in Enum.GetValues(typeof(SensorType)))
            {
                var sensors = hardware.Sensors.Where(s => s.SensorType == (TMSensorType)key);
                sensorDictionary.Add((TMSensorType)key, sensors.ToList());
            }

            return sensorDictionary;
        }
    }
}
