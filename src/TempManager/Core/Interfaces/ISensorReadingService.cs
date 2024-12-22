using Domain.Models;

namespace TempManager.Core.Interfaces
{
    public interface ISensorReadingService
    {
        public void UpdateTrackedSensors(IList<TMSensor> sensors);
    }
}
