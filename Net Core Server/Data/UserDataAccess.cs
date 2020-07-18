using Net_Core_Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Net_Core_Server.Data
{
    public class UserDataAccess
    {
        UserContext context;
        public UserDataAccess(UserContext context)
        {
            this.context = context;
        }

        public async Task<User> AddNewUser(string username)
        {
            var t = context.Add(new User() { UserName = username });
            await context.SaveChangesAsync();
            return t.Entity;
        }
        public bool Contains(string username)
        {
            return context.Users.Any(user => user.UserName == username);
        }
        public bool Contains(Guid apiKey)
        {
            return context.Users.Any(user => user.ApiKey == apiKey);
        }
        public bool Contains(Guid apiKey, string username)
        {
            return context.Users.Any(user => user.ApiKey == apiKey && user.UserName == username);
        }
        public User TryGet(Guid apiKey)
        {
            return context.Users.FirstOrDefault(user => user.ApiKey == apiKey);
        }
        public async Task Remove(Guid apiKey)
        {
            var first = context.Users.FirstOrDefault(user => user.ApiKey == apiKey);
            if(first != null) 
            {
                context.Users.Remove(first);
                await context.SaveChangesAsync();
            }
        }
    }
}
