using Microsoft.Extensions.DependencyInjection;
using Logger.Interfaces;
using Logger.Services;
using Logger.Utilities;
using Notifier.Interfaces;
using Notifier.Services;
using Notifier.Utilities;
using TempManager.UI.Services;
using TempManager.Core.Interfaces;
using TempManager.Core.Services;
using Domain.Models;

namespace TempManager.Application 
{
    class Program
    {
        private static IHardwareService _hardwareService;
        private static ISensorReadingService _sensorReadingService;
        private static TMOverlay _overlayService;

        private static Task InitializeLogger(IServiceProvider serviceProvider)
        {
            Log.Initialize(serviceProvider.GetRequiredService<ICustomLogger>());
            Log.Debug("Logger initialized");

            return Task.CompletedTask;
        }
        private static Task InitializeEventService(IServiceProvider serviceProvider)
        {
            Notify.Initialize(serviceProvider.GetRequiredService<INotifier>());
            Log.Debug("Notifier initialized");
            return Task.CompletedTask;
        }
        private static Task RegisterEvents() 
        {
            if (_sensorReadingService != null) {
                Notify.RegisterEvent("SelectedSensorsChange", (Action<IList<TMSensor>>)_sensorReadingService.CheckTrackedSensors);
                Notify.RegisterEvent("SensorReadingUpdated", (Action<IList<TMSensor>>)_sensorReadingService.UpdateTrackedSensors);
            }
            
            return Task.CompletedTask;
        }
        private static Task InitializeHardwareService()
        {
            _hardwareService = new HardwareService();
            Log.Debug("Hardware service started");
            return Task.CompletedTask;
        }
        private static async Task RunOverlay()
        {
            _overlayService = new TMOverlay(_hardwareService);
            Log.Debug("Overlay started");
            await _overlayService.Run();
        }

        static async Task Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<ICustomLogger, CustomLogger>()
                .AddSingleton<INotifier, CustomNotifier>()
                .BuildServiceProvider();

            await InitializeLogger(serviceProvider);
            await InitializeHardwareService();
            await InitializeEventService(serviceProvider);

            _sensorReadingService = new SensorReadingService();
            await RegisterEvents();
            RunOverlay();

            Log.Info("Application components started");
        }
    }
}