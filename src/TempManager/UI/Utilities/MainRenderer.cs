using ImGuiNET;
using Shared.Domain.Models;
using TempManager.Core.Services;
using Logger.Utilities;

namespace TempManager.UI.Utilities
{
    public class MainRenderer
    {
        public Dictionary<string, List<TMSensor>> selectedValues { get; set; }

        public MainRenderer()
        {
            selectedValues = new Dictionary<string, List<TMSensor>>();
        }

        public void RenderHardware(HardwareService hardwareService)
        {
            foreach (var hardware in hardwareService.GetHardwareComponents())
            {
                // TODO: Render hardware using ImGui.TreeNode()
                if (ImGui.CollapsingHeader(hardware.Name))
                {
                    RenderSensorTypes(hardwareService.GetGroupedSensors(hardware), hardware.Name);
                }
            }
        }
        public void RenderSensorTypes(Dictionary<TMSensorType, List<TMSensor>> groupedSensors, string identifier)
        {
            // Base flags for TreeNode
            ImGuiTreeNodeFlags baseFlags =
                ImGuiTreeNodeFlags.OpenOnArrow
                | ImGuiTreeNodeFlags.OpenOnDoubleClick
                | ImGuiTreeNodeFlags.SpanAvailWidth;
            // Dictionary to store a lastly clicked node
            var clickedNode = new Dictionary<string, List<TMSensor>>();

            foreach (var sensorType in groupedSensors.Keys)
            {
                if (groupedSensors[sensorType].Count <= 0) { continue; }

                ImGuiTreeNodeFlags nodeFlags = baseFlags;
                string hardwareName = groupedSensors[sensorType][0].HardwareName;
                // Apply selected flag if sensor is in selectedValues
                if (selectedValues.ContainsKey($"{hardwareName} - {sensorType}"))
                {
                    nodeFlags |= ImGuiTreeNodeFlags.Selected;
                }

                // Render sensor type
                bool isOpen = ImGui.TreeNodeEx($"{sensorType.ToString()}##{identifier}", nodeFlags);

                if (ImGui.IsItemClicked() && !ImGui.IsItemToggledOpen())
                {
                    Log.Info($"Clicked on {hardwareName} - {sensorType}");
                    clickedNode[$"{hardwareName} - {sensorType}"] = groupedSensors[sensorType];
                }

                // If sensor type is open, render sensors
                if (isOpen) RenderSensors(groupedSensors[sensorType]);
            }

            // Update selectedValues with clicked node
            if (clickedNode.Count > 0)
            {
                foreach (var node in clickedNode)
                {
                    if (selectedValues.ContainsKey(node.Key))
                    {
                        selectedValues.Remove(node.Key);
                    }
                    else
                    {
                        selectedValues.Add(node.Key, node.Value);
                    }
                }
            }
        }
        public void RenderSensors(List<TMSensor> sensors)
        {
            foreach (var sensor in sensors)
            {
                ImGui.Text($"{sensor.Name}: {sensor.Value?.ToString("F2")}");
            }
            ImGui.TreePop();
        }
    }
}
