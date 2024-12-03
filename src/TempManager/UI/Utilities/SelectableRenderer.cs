using ImGuiNET;
using Domain.Models;

namespace TempManager.UI.Utilities
{
    public class SelectableRenderer
    {
        public bool isCollapsed { get; set; } = true;

        public void Render(Dictionary<string, List<TMSensor>> selectedValues, Dictionary<TMSensor, LinePlotRenderer> plottedSensors)
        {
            ImGui.SetNextWindowBgAlpha(0.7f);
            ImGui.Begin(
                "Selected Sensors",
                ImGuiWindowFlags.AlwaysAutoResize |
                ImGuiWindowFlags.NoResize |
                ImGuiWindowFlags.NoDocking
                );

            foreach (string key in selectedValues.Keys)
            {
                bool isOpen = ImGui.TreeNode($"{key}");
                if (!isOpen) continue;

                foreach (TMSensor sensor in selectedValues[key])
                {
                    string _label = $"{sensor.Name} - {sensor.Value?.ToString("F2")}";
                    if (plottedSensors.ContainsKey(sensor))
                    {
                        plottedSensors[sensor].Render(_label);
                    }
                    else
                    {
                        ImGui.Text(_label);
                    }
                }
                ImGui.TreePop();
            }
        }
    }
}
