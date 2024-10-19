namespace TempManager
{
    using System.Threading.Tasks;
    class Program
    {
        static Task RetrieveHardware()
        {
            MonitorManager monitorManager = new MonitorManager();
            monitorManager.Monitor();
            
            return Task.CompletedTask;
        }
        static async Task RunOverlay()
        {
            using var overlay = new TempManagerOverlay();
            await overlay.Run();
        }
        static async Task Main()
        {
            await RetrieveHardware();
            await RunOverlay();
        }
    }
}