using System;

namespace HTTP_Client_Asp_Server.Infrastructure
{
    public class ConsoleOutput : ILogger
    {
        public void Log(object message, LogType logType)
        {
            var color = logType switch
            {
                LogType.Info => ConsoleColor.White,
                LogType.Debug => ConsoleColor.Cyan,
                LogType.Warning => ConsoleColor.Yellow,
                LogType.Error => ConsoleColor.Red,
                _ => throw new NotImplementedException(),
            };
            Print(message, color);
        }

        public void Print(object message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        public void Clear()
        {
            Console.Clear();
        }
    }
}