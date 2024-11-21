using Logger.Interfaces;

namespace Logger.Services
{
    public class CustomLogger : ICustomLogger
    {
        public void Info(string message)
        {
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("INFO");
            Console.ResetColor();
            Console.WriteLine($"] {DateTime.Now}: {message}");
        }
        public void Warn(string message)
        {
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("WARN");
            Console.ResetColor();
            Console.WriteLine($"] {DateTime.Now}: {message}");
        }
        public void Error(string message, Exception exception = null)
        {
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("ERROR");
            Console.ResetColor();
            Console.WriteLine($"] {DateTime.Now}: {message}");
            if (exception != null)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
