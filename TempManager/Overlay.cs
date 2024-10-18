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

    internal class TempManagerOverlay : Overlay
    {
        private bool isRunning = true;

        public TempManagerOverlay() : base("TempManagerOverlay", true, 3840, 2160)
        {
        }

        protected override void Render()
        {

            ImGui.Begin("TempManager", ref isRunning, ImGuiWindowFlags.AlwaysAutoResize);
            ImGui.Text("Hello, world!");
            ImGui.NewLine();
            ImGui.Text("This will one day be a usefull resource manager.");
            ImGui.End();

            if(!isRunning)
            {
                Close();
            }
        }
    }
}