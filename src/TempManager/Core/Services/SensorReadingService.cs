using TempManager.Core.Interfaces;
using Domain.Models;

namespace TempManager.Core.Services
{
    public class SensorReadingService : ISensorReadingService
    {
        private IList<SensorReading> _readings;

        public SensorReadingService() 
        {
            _readings = new List<SensorReading>();
        }

        public void UpdateTrackedSensors(IList<TMSensor> sensors) 
        {
            // TODO: Implement this method
            return;
        }
    }
}
