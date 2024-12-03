using Domain.Models;
using ImGuiNET;

namespace TempManager.UI.Utilities
{
    public class LinePlotRenderer
    {
        private TMSensor _sensor;
        private float _sensorData;

        static int _valuesOffset = 0;
        static double _refreshTime = 0.0;

        public LinePlotRenderer(TMSensor sensor)
        {
            _sensor = sensor;
            _sensorData = 0.0f;
        }

        public void Render()
        {
            {
                double avg = 0.0;
                foreach (float value in this._sensorData)
                    avg += value;
                avg /= this._sensorData.Length;
                ImGui.PlotLines("Sensor data plot", ref this._sensorData, 3);
            }
        }
    }
}
