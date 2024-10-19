namespace TempManager
{
    using System.Collections.Generic;
    using ClickableTransparentOverlay;
    using ImGuiNET;
    using SixLabors.ImageSharp.PixelFormats;
    using SixLabors.ImageSharp;
    using System.Threading.Tasks;
    using System.Numerics;
    using System;
    using System.Drawing;
    using System.IO;
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
                if (ImGui.CollapsingHeader(hardware.Name))
                {
                    foreach(var sensor in hardware.Sensors)
                    {
                        ImGui.Text($"{sensor.SensorType} - {sensor.Name}: {sensor.Value.ToString()}");
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

            if(!_isRunning)
            {
                Close();
            }
        }
    }
}