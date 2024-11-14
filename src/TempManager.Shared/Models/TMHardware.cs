
using LibreHardwareMonitor.Hardware;

namespace TempManager.Shared.Models
{
    public class TMHardware
    {
        private IHardware _hardware;
        public string Name { get; set; }
        public string HardwareType { get; set; }
        public TMSensor[] Sensors { get; set; }
        public TMHardware[] SubHardware { get; set; }
        public TMHardware(IHardware hardware)
        {
            _hardware = hardware;
            Name = hardware.Name;
            HardwareType = hardware.HardwareType.ToString();
            Sensors = hardware.Sensors.Select(sensor => new TMSensor(sensor)).ToArray();
            SubHardware = hardware.SubHardware.Select(subHardware => new TMHardware(subHardware)).ToArray();
        }
        public void Update()
        {
            _hardware.Update();
            foreach (var hardware in SubHardware) hardware.Update();
            foreach (var sensor in Sensors) sensor.Update();
        }
    }
}
