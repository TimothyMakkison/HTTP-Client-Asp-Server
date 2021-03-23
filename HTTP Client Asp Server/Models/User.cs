using System;

namespace HTTP_Client_Asp_Server.Models
{
    public class User
    {
        public Guid ApiKey { get; set; }
        public string Username { get; set; }

        public override string ToString() => $"User ApiKey: {ApiKey}, Username: {Username}";
    }
}