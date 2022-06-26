using System;

namespace Client.Infrastructure;

public interface ILogger
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
