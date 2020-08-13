namespace HTTP_Client_Asp_Server.Handlers
{
    public interface ICommandData
    {
        string CommandKey { get; }
        ParseMode Parsing { get; set; }
    }
}