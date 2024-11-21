using LibreHardwareMonitor.Hardware;
using Shared.Domain.Models;

namespace Shared.Utilities
{
    public static class SensorTypeMapper
    {
        public static TMSensorType ToTMSensorType(SensorType sensorType)
        {
            return sensorType switch
            {
                SensorType.Voltage => TMSensorType.Voltage,
                SensorType.Current => TMSensorType.Current,
                SensorType.Power => TMSensorType.Power,
                SensorType.Clock => TMSensorType.Clock,
                SensorType.Temperature => TMSensorType.Temperature,
                SensorType.Load => TMSensorType.Load,
                SensorType.Frequency => TMSensorType.Frequency,
                SensorType.Fan => TMSensorType.Fan,
                SensorType.Flow => TMSensorType.Flow,
                SensorType.Control => TMSensorType.Control,
                SensorType.Level => TMSensorType.Level,
                SensorType.Factor => TMSensorType.Factor,
                SensorType.Data => TMSensorType.Data,
                SensorType.SmallData => TMSensorType.SmallData,
                SensorType.Throughput => TMSensorType.Throughput,
                SensorType.TimeSpan => TMSensorType.TimeSpan,
                SensorType.Energy => TMSensorType.Energy,
                SensorType.Noise => TMSensorType.Noise,
                _ => throw new ArgumentOutOfRangeException(nameof(sensorType), sensorType, null)
            };
        }
    }
}
