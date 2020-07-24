using Microsoft.EntityFrameworkCore;
using Net_Core_Server.Models;
using System;
using System.Threading.Tasks;

namespace Net_Core_Server.Data
{
    public class UserDataAccess
    {
        private readonly UserContext context;

        public UserDataAccess(UserContext context)
        {
            this.context = context;
        }

        public async Task<User> AddNewUser(string username)
        {
            var role = await context.Users.CountAsync() > 0 ? Role.User : Role.Admin;
            var newUser = await context.AddAsync(new User() { UserName = username, Role = role });
            await context.SaveChangesAsync();
            return newUser.Entity;
        }

        public async Task<bool> ContainsUsername(string username) => await context.Users.AnyAsync(user => user.UserName == username);

        public async Task<bool> Contains(Guid apiKey) => await context.Users.AnyAsync(user => user.ApiKey == apiKey);

        public async Task<bool> Contains(Guid apiKey, string username) => await context.Users.AnyAsync(user => user.ApiKey == apiKey && user.UserName == username);

        public async Task<User> TryGet(Guid apiKey) => await context.Users.FirstOrDefaultAsync(user => user.ApiKey == apiKey);

        public async Task<bool> Remove(Guid apiKey)
        {
            var first = await context.Users.FirstOrDefaultAsync(user => user.ApiKey == apiKey);
            if (first != null)
            {
                context.Users.Remove(first);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> ChangeRole(string username, string role)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.UserName == username);
            if (user != null)
            {
                user.Role = role;
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}