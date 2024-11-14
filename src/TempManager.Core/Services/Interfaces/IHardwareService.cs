using LibreHardwareMonitor.Hardware;
using TempManager.Shared.Models;

namespace TempManager.Core.Services
{
    public interface IHardwareService
    {
        Task Update();
        IList<TMHardware> GetHardwareComponents();
    }
}
