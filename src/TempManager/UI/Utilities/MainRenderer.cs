using ImGuiNET;
using Domain.Models;
using TempManager.Core.Services;
using Logger.Utilities;

namespace TempManager.UI.Utilities
{
    public class MainRenderer
    {
        public Dictionary<string, List<TMSensor>> selectedValues { get; set; }
        public Dictionary<TMSensor, LinePlotRenderer> plottedSensors { get; set; }

        public MainRenderer()
        {
            selectedValues = new Dictionary<string, List<TMSensor>>();
            plottedSensors = new Dictionary<TMSensor, LinePlotRenderer>();
        }
        private void UpdateSelectedSensors(string svIndex, TMSensor sensor)
        {
            if (selectedValues.ContainsKey(svIndex)
                && selectedValues[svIndex].Contains(sensor))
            {
                selectedValues[svIndex].Remove(sensor);

                if (selectedValues[svIndex].Count <= 0)
                    selectedValues.Remove(svIndex);
            }
            else
            {
                if (selectedValues.ContainsKey(svIndex)) 
                    selectedValues[svIndex].Add(sensor);
                else 
                    selectedValues.Add(svIndex, new List<TMSensor> { sensor });
            }
        }
        private void RenderSensorContextMenu(TMSensor sensor)
        {
            if (ImGui.BeginPopupContextItem($"SensorContextMenu###{sensor.Name}"))
            {
                var _svIndex = $"{sensor.HardwareName} - {sensor.SensorType}";
                // Render context menu items

                // Select sensor
                if (ImGui.Selectable("Select"))
                {
                    UpdateSelectedSensors(_svIndex, sensor);
                }

                // Plot sensor
                if  (ImGui.Selectable("Plot"))
                {
                    // If sensor isn't selected, add it to selectedValues
                    if (!selectedValues.ContainsKey(_svIndex) || !selectedValues[_svIndex].Contains(sensor))
                    {
                        UpdateSelectedSensors(_svIndex, sensor);
                    }
                    
                    // Add / remove sensor from plottedSensors
                    if (plottedSensors.ContainsKey(sensor))
                        plottedSensors.Remove(sensor);
                    else
                        plottedSensors.Add(sensor, new LinePlotRenderer(sensor));
                }

                // Close context menu
                if (ImGui.Button("Close"))
                {
                    ImGui.CloseCurrentPopup();
                }
                ImGui.EndPopup();
            }
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
                if (groupedSensors[sensorType].Count <= 0) continue;

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
            if (clickedNode.Count <= 0) return;

            foreach (var node in clickedNode)
            {
                if (selectedValues.ContainsKey(node.Key))
                {
                    selectedValues.Remove(node.Key);
                }
                else selectedValues.Add(node.Key, node.Value);
            }
        }
        public void RenderSensors(List<TMSensor> sensors)
        {
            foreach (var sensor in sensors)
            {
                ImGui.Text($"{sensor.Name}: {sensor.Value?.ToString("F2")}");
                RenderSensorContextMenu(sensor);
            }
            ImGui.TreePop();
        }
    }
}
