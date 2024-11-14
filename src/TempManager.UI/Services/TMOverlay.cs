using TempManager.Shared.Utilities;
using TempManager.Core.Services;
using TempManager.UI.Utilities;
using ImGuiNET;
using ClickableTransparentOverlay;
using Coroutine;
using System.Numerics;

namespace TempManager.UI.Services
{
    public class TMOverlay: Overlay
    {
        private readonly HardwareService _hardwareService;
        private readonly MainRenderer _mainRenderer = new MainRenderer();
        private readonly SelectableRenderer _selectableRenderer = new SelectableRenderer();

        private readonly ActiveCoroutine _monitorUpdateCoroutine;
        private readonly double _hardwareUpdateDelay = 0.5;
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
            ImGui.SetNextWindowSizeConstraints(new Vector2(400, 200), new Vector2(1920, 1080));
            bool isCollapsed = !ImGui.Begin(
                "TempManager - Hardware and Sensors", 
                ref _isRunning,
                ImGuiWindowFlags.AlwaysAutoResize |
                ImGuiWindowFlags.AlwaysVerticalScrollbar
                );

            if (!_isRunning || isCollapsed)
            {
                ImGui.End();
                if (! _isRunning) Close();

                return;
            }

            _mainRenderer.RenderHardware(_hardwareService);

            if (_mainRenderer.selectedValues.Count > 0)
            {
                _selectableRenderer.Render(_mainRenderer.selectedValues);
            }

            ImGui.End();

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
    }
}
