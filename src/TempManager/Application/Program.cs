using Microsoft.Extensions.DependencyInjection;
using Logger.Interfaces;
using Logger.Services;
using Logger.Utilities;
using TempManager.Core.Services;
using TempManager.UI.Services;

namespace TempManager.Application 
{
    class Program
    {
        private static HardwareService _hardwareService;
        private static TMOverlay _overlayService;

        private static Task InitializeLogger()
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<ICustomLogger, CustomLogger>()
                .BuildServiceProvider();

            Log.Initialize(serviceProvider.GetRequiredService<ICustomLogger>());
            Log.Info("Logger initialized");

            return Task.CompletedTask;
        }
        private static Task InitializeHardwareService()
        {
            _hardwareService = new HardwareService();
            Log.Info("Hardware service started");
            return Task.CompletedTask;
        }

        private static async Task RunOverlay()
        {
            _overlayService = new TMOverlay(_hardwareService);
            Log.Info("Overlay started");
            await _overlayService.Run();
        }

        static async Task Main(string[] args)
        {
            await InitializeLogger();
            await InitializeHardwareService();
            await RunOverlay();
        }
    }
}