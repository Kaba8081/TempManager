using TempManager.Core.Services;
using TempManager.UI.Services;

namespace TempManager.App.Program
{
    class Program
    {
        private static HardwareService _hardwareService;
        private static TMOverlay _overlayService;

        private static Task InitializeHardwareService()
        {
            _hardwareService = new HardwareService();
            return Task.CompletedTask;
        }

        private static async Task RunOverlay()
        {
            _overlayService = new TMOverlay(_hardwareService);
            await _overlayService.Run();
        }

        static async Task Main(string[] args)
        {
            await InitializeHardwareService();
            await RunOverlay();
        }
    }
}