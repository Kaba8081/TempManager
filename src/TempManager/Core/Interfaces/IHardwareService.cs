using Domain.Models;

namespace TempManager.Core.Interfaces
{
    public interface IHardwareService
    {
        Task Update();
        IList<TMHardware> GetHardwareComponents();
        Dictionary<TMSensorType, List<TMSensor>> GetGroupedSensors(TMHardware hardware);
    }
}
