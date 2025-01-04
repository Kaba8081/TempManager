using Domain.Models;

namespace TempManager.Core.Interfaces
{
    public interface ISensorReadingService
    {
        public void CheckTrackedSensors(IList<TMSensor> new_sensors);
        public void UpdateTrackedSensors(IList<TMSensor> sensors);
    }
}
