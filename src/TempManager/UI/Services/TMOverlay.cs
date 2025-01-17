﻿using System.Numerics;
using Coroutine;
using ImGuiNET;
using ClickableTransparentOverlay;
using TempManager.Core.Services;
using TempManager.Core.Interfaces;
using TempManager.UI.Utilities;

namespace TempManager.UI.Services
{
    public class TMOverlay : Overlay
    {
        private readonly IHardwareService _hardwareService;
        private readonly MainRenderer _mainRenderer;
        private readonly SelectableRenderer _selectableRenderer;

        private bool _isRunning = true;

        public TMOverlay() : base("TempManager", true, 3840, 2160)
        {
            _hardwareService = new HardwareService();
            _mainRenderer = new MainRenderer();
            _selectableRenderer = new SelectableRenderer();
            
        } 
        public TMOverlay(IHardwareService hardwareService) : base("TempManager", true, 3840, 2160)
        {
            _hardwareService = hardwareService;
            _mainRenderer = new MainRenderer();
            _selectableRenderer = new SelectableRenderer();
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

            if (!_isRunning)
            {
                ImGui.End();
                Close();
                return;
            }

            _mainRenderer.RenderHardware(_hardwareService);

            if (_mainRenderer.selectedValues.Count > 0)
            {
                _selectableRenderer.Render(_mainRenderer.selectedValues, _mainRenderer.plottedSensors);
            }

            ImGui.End();

        }
    }
}
