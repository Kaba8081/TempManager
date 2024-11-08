using Domain.Entities;

namespace TempManagerOverlay
{
    class Program
    {
        private static MonitorManager _monitorManager = null!;
        private static TempManagerOverlay _overlayManager = null!;

        private static Task InitializeHardware()
        {
            _monitorManager = new MonitorManager();

            return Task.CompletedTask;
        }
        static async Task RunOverlay()
        {
            _overlayManager = new TempManagerOverlay(_monitorManager);
            await _overlayManager.Run();
        }
        static async Task Main()
        {
            await InitializeHardware();
            await RunOverlay();
        }
    }
}