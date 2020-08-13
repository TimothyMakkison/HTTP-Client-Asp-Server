namespace HTTP_Client_Asp_Server.Handlers
{
    [System.AttributeUsage(System.AttributeTargets.Method)
]
    public class CommandAttribute : System.Attribute, ICommandData
    {
        public ParseMode Parsing { get; set; } = ParseMode.None;

        public string CommandKey { get; }

        public CommandAttribute(string command)
        {
            this.CommandKey = command;
        }
    }
}