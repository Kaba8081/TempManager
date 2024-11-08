namespace TempManagerOverlay
{
    using System.Threading.Tasks;
    using Domain.Entities;
    class Program
    {
        private static MonitorManager _monitorManager = null!;

        private static Task InitializeHardware()
        {
            _monitorManager = new MonitorManager();

            return Task.CompletedTask;
        }
        static async Task RunOverlay()
        {
            using var overlay = new TempManagerOverlay(_monitorManager);
            await overlay.Run();
        }
        static async Task Main()
        {
            await InitializeHardware();
            await RunOverlay();
        }
    }
}