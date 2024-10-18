namespace TempManager
{
    using System.Threading.Tasks;
    class Program
    {
        static async Task RunOverlay()
        {
            using var overlay = new TempManagerOverlay();
            await overlay.Run();
        }
        static async Task Main()
        {
            await RunOverlay();
        }
    }
}