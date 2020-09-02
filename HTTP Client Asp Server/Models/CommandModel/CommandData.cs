using HTTP_Client_Asp_Server.Handlers;

namespace HTTP_Client_Asp_Server.Models.CommandModel
{
    public class CommandData : ICommandData
    {
        public string CommandKey { get; set; }

        public ParseMode Parsing { get; set; }
    }
}