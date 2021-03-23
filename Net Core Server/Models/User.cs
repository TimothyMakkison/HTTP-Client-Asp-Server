using System;
using System.ComponentModel.DataAnnotations;

namespace Net_Core_Server.Models
{
    public class User
    {
        public User()
        {
        }

        [Key]
        public Guid ApiKey { get; set; }

        public string UserName { get; set; }
        public string Role { get; set; }
    }
}