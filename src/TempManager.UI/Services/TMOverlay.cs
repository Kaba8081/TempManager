using TempManager.Shared.Utilities;
using TempManager.Core.Services;
using ImGuiNET;
using ClickableTransparentOverlay;
using Coroutine;


namespace TempManager.UI.Services
{
    public class TMOverlay: Overlay
    {
        private readonly HardwareService _hardwareService;
        private readonly ActiveCoroutine _monitorUpdateCoroutine;
        private readonly int _hardwareUpdateDelay = 5;
        private bool _isRunning = true;

        public TMOverlay() : base("TempManager", true, 3840, 2160)
        {
            _hardwareService = new HardwareService();
            _monitorUpdateCoroutine = CoroutineHandler.Start(UpdateHardware(), name: "HardwareUpdateCoroutine");
        }
        public TMOverlay(HardwareService hardwareService) : base("TempManager", true, 3840, 2160)
        {
            _hardwareService = hardwareService;
            _monitorUpdateCoroutine = CoroutineHandler.Start(UpdateHardware(), name: "HardwareUpdateCoroutine");
        }
        protected override void Render()
        {
            CoroutineHandler.Tick(ImGui.GetIO().DeltaTime);
            ImGui.Begin("TempManager - Hardware and Sensors", ref _isRunning, ImGuiWindowFlags.AlwaysAutoResize);

            RenderHardware();

            ImGui.End();

            if (!_isRunning)
            {
                Close();
            }
        }
        private IEnumerable<Wait> UpdateHardware()
        {
            Log.Info("UpdateHardware coroutine started");
            while (this._isRunning)
            {
                this._hardwareService.Update().Wait();
                yield return new Wait(this._hardwareUpdateDelay);
            }
        }

        private void RenderHardware()
        {
            foreach (var hardware in _hardwareService.GetHardwareComponents())
            {
                // TODO: Render hardware using ImGui.TreeNode()
                if (ImGui.CollapsingHeader(hardware.Name))
                    {
                    var GroupedSensorsDictionary = _hardwareService.GetGroupedSensors(hardware);

                    foreach (var sensorType in GroupedSensorsDictionary.Keys)
                    {
                        if (GroupedSensorsDictionary[sensorType].Count <= 0) { continue; }

                        if (ImGui.TreeNode(sensorType.ToString()))
                        {
                            foreach (var sensor in GroupedSensorsDictionary[sensorType])
                            {
                                ImGui.Text($"{sensorType} - {sensor.Name}: {sensor.Value?.ToString("F2")}");
                            }
                            ImGui.TreePop();
                        }
                    }
                }
            }
        }
    }
}
