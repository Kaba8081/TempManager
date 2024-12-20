using Domain.Models;
using Coroutine;
using ImGuiNET;
using Logger.Utilities;
using System.Numerics;

namespace TempManager.UI.Utilities
{
    public class LinePlotRenderer
    {
        private TMSensor _sensor;
        private List<float> _sensorData;
        private List<float> _smoothedSensorData;

        // Add to config
        static int _valuesCount = 100;
        static double _refreshTime = 0.5;
        static Vector2 _plotSize = new Vector2(0, 50);

        public LinePlotRenderer(TMSensor sensor)
        {
            _sensor = sensor;
            _sensorData = new List<float>();
            _smoothedSensorData = new List<float>();
            CoroutineHandler.Start(UpdateSensorData());
        }

        private IEnumerable<Wait> UpdateSensorData()
        {
            Log.Info("UpdateSensorData coroutine started for sensor: " + _sensor.Name);
            while (true)
            {
            if (_sensorData.Count >= _valuesCount)
            {
                _sensorData.RemoveAt(0);
            }
            _sensorData.Add((float)(_sensor.Value ?? 0.0));

            _smoothedSensorData = new List<float>(_sensorData);
            // TODO Implement a better smoothing algorithm
            for (int i = 0; i < _sensorData.Count; i++)
            {
                float sum = 0;
                int count = 0;
                for (int j = Math.Max(0, i - 5); j <= Math.Min(_sensorData.Count - 1, i + 5); j++)
                {
                    sum += (float)Math.Log(_sensorData[j] + 1); // Adding 1 to avoid log(0)
                    count++;
                }
                _smoothedSensorData[i] = (float)Math.Exp(sum / count) - 1; // Subtracting 1 to revert the previous addition
            }

            yield return new Wait(_refreshTime);
            }
        }

        public void Render(string? label = "Sensor data plot")
        {
            double avg = 0.0;
            foreach (float value in _smoothedSensorData)
                avg += value;
            if (_smoothedSensorData.Count > 0)
                avg /= _smoothedSensorData.Count;
            var sensorDataArray = _smoothedSensorData.ToArray();
            ImGui.PlotLines(label, ref sensorDataArray[0], _smoothedSensorData.Count, 0, null, _smoothedSensorData.Min(), _smoothedSensorData.Max(), _plotSize);
        }
    }
}
