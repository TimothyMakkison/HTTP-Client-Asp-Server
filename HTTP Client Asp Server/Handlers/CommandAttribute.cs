namespace HTTP_Client_Asp_Server.Handlers
{
    [System.AttributeUsage(System.AttributeTargets.Method)
]
    public class CommandAttribute : System.Attribute, ICommandData
    {
        public bool Parsing { get; set; } = false;

        public string CommandKey { get; }

        public CommandAttribute(string command)
        {
            this.CommandKey = command;
        }
    }
}