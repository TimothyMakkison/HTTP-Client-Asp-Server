using System;
using System.ComponentModel.DataAnnotations;

namespace Net_Core_Server.Models;

public class User
{
    public User()
    {
    }

    [Key]
    public Guid ApiKey { get; init; }

    public string UserName { get; init; }
    public string Role { get; set; }
}
