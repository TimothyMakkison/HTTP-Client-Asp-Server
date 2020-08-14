namespace HTTP_Client_Asp_Server.Handlers
{
    public static class CommandInvoker
    {
        public static void Invoke(CommandModel command, string input)
        {
            string values = command.Parsing switch
            {
                ParseMode.None => input,
                ParseMode.Parse => input.Replace(command.CommandKey, ""),
                ParseMode.ParseAndTrim => input.Replace(command.CommandKey, "").Trim(' '),
                _ => input,
            };

            command.Operation.Invoke(values).Wait();
        }
    }
}