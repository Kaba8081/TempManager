using Shared.Domain.Models;

namespace TempManager.Core.Interfaces
{
    public interface IHardwareService
    {
        Task Update();
        IList<TMHardware> GetHardwareComponents();
    }
}
