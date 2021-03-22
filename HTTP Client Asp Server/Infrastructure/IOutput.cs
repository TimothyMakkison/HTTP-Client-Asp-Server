using System;

namespace HTTP_Client_Asp_Server.Infrastructure
{
    public interface IOutput
    {
        public void Print(object message, ConsoleColor color = ConsoleColor.White);
        public void Log(object message, LogType logType = LogType.Info);
        public void Clear();
    }
    public enum LogType
    {
        Error,
        Warning,
        Info,
        Debug,
    }
}