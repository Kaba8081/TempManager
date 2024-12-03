using ImGuiNET;
using Domain.Models;

namespace TempManager.UI.Utilities
{
    public class SelectableRenderer
    {
        public bool isCollapsed { get; set; } = true;

        public void Render(Dictionary<string, List<TMSensor>> selectedValues)
        {
            ImGui.SetNextWindowBgAlpha(0.7f);
            ImGui.Begin(
                "Selected Sensors",
                ImGuiWindowFlags.AlwaysAutoResize |
                ImGuiWindowFlags.NoResize |
                ImGuiWindowFlags.NoDocking
                );

            foreach (var key in selectedValues.Keys)
            {
                bool isOpen = ImGui.TreeNode($"{key}");
                if (!isOpen) continue;

                foreach (var sensor in selectedValues[key])
                {
                    ImGui.Text($"{sensor.Name} - {sensor.Value?.ToString("F2")}");
                }
                ImGui.TreePop();
            }
        }
    }
}
