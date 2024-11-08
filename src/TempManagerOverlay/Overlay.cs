namespace TempManagerOverlay
{
    using Domain.Entities;
    using System.Collections.Generic;
    using ClickableTransparentOverlay;
    using ImGuiNET;
    using System;
    using Coroutine;

    internal class TempManagerOverlay : Overlay
    {
        private readonly MonitorManager _monitorManager;
        private readonly ActiveCoroutine _monitorUpdateCoroutine;
        private bool _isRunning = true;
        private readonly int _hardwareUpdateDelay = 5;


        public TempManagerOverlay(MonitorManager manager) : base("TempManagerOverlay", true, 3840, 2160)
        {
            _monitorManager = manager;
            _monitorUpdateCoroutine = CoroutineHandler.Start(UpdateHardware(), name: "MonitorUpdate");
        }
        private IEnumerable<Wait> UpdateHardware()
        {
            // TODO: Update only visible hardware
            // ? maybe send a list of visible hardware to the coroutine
            Console.WriteLine("Starting Hardware Update Coroutine");
            while (this._isRunning)
            {
                this._monitorManager.Update().Wait();
                yield return new Wait(this._hardwareUpdateDelay);
            }

        }

        private void RenderHardware()
        {
            foreach (var hardware in _monitorManager.GetHardware())
            {
                // TODO: Render hardware using ImGui.TreeNode()
                if (ImGui.CollapsingHeader(hardware.Name))
                {
                    var hardwareSensors = _monitorManager.GetSensors(hardware);

                    foreach (var sensorType in hardwareSensors.Keys)
                    {
                        if (hardwareSensors[sensorType].Count <= 0) { continue; }

                        if (ImGui.TreeNode(sensorType.ToString()))
                        {
                            foreach (var sensor in hardwareSensors[sensorType])
                            {
                                ImGui.Text($"{sensorType} - {sensor.Name}: {sensor.Value?.ToString("F2")}");
                            }
                            ImGui.TreePop();
                        }
                    }
                }
            }
        }

        protected override void Render()
        {
            CoroutineHandler.Tick(ImGui.GetIO().DeltaTime);

            ImGui.Begin("TempManager", ref _isRunning, ImGuiWindowFlags.AlwaysAutoResize);
            ImGui.Text("Hello, world!");
            ImGui.NewLine();
            RenderHardware();
            ImGui.NewLine();
            ImGui.Text("This will one day be a usefull resource manager.");
            ImGui.End();

            if (!_isRunning)
            {
                Close();
            }
        }
    }
}
