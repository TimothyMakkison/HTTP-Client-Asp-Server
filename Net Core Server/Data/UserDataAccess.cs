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
            var role = context.Users.Count() > 0 ? Role.User : Role.Admin;
            var newUser = context.Add(new User() { UserName = username, Role = role });
            await context.SaveChangesAsync();
            return newUser.Entity;
        }
        public bool ContainsUsername(string username)
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
