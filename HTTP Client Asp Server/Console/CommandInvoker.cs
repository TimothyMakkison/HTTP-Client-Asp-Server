namespace HTTP_Client_Asp_Server.Handlers
{
    public static class CommandInvoker
    {
        public static void Invoke(CommandModel command, string input)
        {
            string values = command.Data.Parsing switch
            {
                ParseMode.None => input,
                ParseMode.Parse => input.Replace(command.Data.CommandKey, ""),
                ParseMode.ParseAndTrim => input.Replace(command.Data.CommandKey, "").Trim(' '),
                _ => input,
            };

            command.Operation.Invoke(values).Wait();
        }
    }
}