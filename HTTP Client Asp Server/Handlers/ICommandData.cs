using System.Runtime.ConstrainedExecution;

namespace HTTP_Client_Asp_Server.Handlers
{
    public interface ICommandData
    {
        string CommandKey { get; }
        bool Parsing { get; set; }
    }
}