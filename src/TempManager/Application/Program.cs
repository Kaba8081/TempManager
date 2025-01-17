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
using Domain.Services;
using Domain.Services.Interfaces;
using Coroutine;

namespace TempManager.Application 
{
    class Program
    {
        private static IHardwareService _hardwareService;
        private static ISensorReadingService _sensorReadingService;
        private static TMOverlay _overlayService;
        private static IFileHandler _fileHandler;

        private static ActiveCoroutine _monitorUpdateCoroutine;
        private static readonly double _hardwareUpdateDelay = 0.5;

        private static IEnumerable<Wait> UpdateHardware()
        {
            Log.Debug("UpdateHardware coroutine started");
            while (true)
            {
                _hardwareService.Update().Wait();
                Notify.TriggerEvent("HardwareUpdated");

                yield return new Wait(_hardwareUpdateDelay);
            }
        }

        private static async Task RunOverlay()
        {
            _overlayService = new TMOverlay(_hardwareService);
            Log.Debug("Overlay started");
            await _overlayService.Run();
        }

        #region InitializationMethonds

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
            int eventAmount = 0;

            if (_sensorReadingService != null) {
                Notify.RegisterEvent("SelectedSensorsChange", (Action<IList<TMSensor>>)_sensorReadingService.CheckTrackedSensors);
                Notify.RegisterEvent("HardwareUpdated", _sensorReadingService.UpdateTrackedSensors);
            }

            Log.Debug($"Finished registering {eventAmount} events");
            return Task.CompletedTask;
        }

        private static Task InitializeHardwareService()
        {
            _hardwareService = new HardwareService();
            _monitorUpdateCoroutine = CoroutineHandler.Start(UpdateHardware(), name: "HardwareUpdateCoroutine");

            Log.Debug("Hardware service started");
            return Task.CompletedTask;
        }

        private static Task InitializeFileHandler() 
        {
            // FileHandler already initialized
            if (_fileHandler != null) 
                return Task.CompletedTask;

            _fileHandler = new FileHandler();

            Log.Debug("FileHandler initialized");
            return Task.CompletedTask;
        }

        #endregion

        static async Task Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += OnExit;

            var serviceProvider = new ServiceCollection()
                .AddSingleton<ICustomLogger, CustomLogger>()
                .AddSingleton<INotifier, CustomNotifier>()
                .BuildServiceProvider();

            await InitializeLogger(serviceProvider);
            await InitializeHardwareService();
            await InitializeEventService(serviceProvider);

            if (args.Length > 0) 
            {
                if (args.Contains("--save"))
                    await InitializeFileHandler();
                    _sensorReadingService = new SensorReadingService(_fileHandler);
            }

            await RegisterEvents();
            RunOverlay();

            Log.Info("Application components started");
        }

        static void OnExit(object sender, EventArgs e) 
        {
            _sensorReadingService?.SaveTrackedSensors();

            _overlayService.Close();
            _monitorUpdateCoroutine.Cancel();
            _sensorReadingService = null;
            _hardwareService = null;

            Log.Info("Application exiting ...");
        }
    }
}