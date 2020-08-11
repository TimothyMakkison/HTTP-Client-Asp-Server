using HTTP_Client_Asp_Server.Models;

namespace HTTP_Client_Asp_Server.Handlers
{
    public class UserHandler : User
    {
        public void SetValues(string username, string apiKey)
        {
            ApiKey = apiKey;
            Username = username;
            Assigned = true;
        }

        public bool Assigned { get; set; } = false;
    }
}