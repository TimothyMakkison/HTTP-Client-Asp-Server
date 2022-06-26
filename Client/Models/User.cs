using System;

namespace Client.Models;

public class User
{
    public Guid ApiKey { get; set; }
    public string Username { get; set; }

    public override string ToString() => $"User ApiKey: {ApiKey}, Username: {Username}";
}
