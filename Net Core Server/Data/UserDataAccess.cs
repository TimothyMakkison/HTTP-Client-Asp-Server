using Microsoft.EntityFrameworkCore;
using Net_Core_Server.Models;
using System;
using System.Threading.Tasks;

namespace Net_Core_Server.Data
{
    public class UserDataAccess : IUserDataAccess
    {
        private readonly UserContext _context;

        public UserDataAccess(UserContext context)
        {
            this._context = context;
        }

        public async Task<User> Add(string username)
        {
            var role = await _context.Users.CountAsync() > 0 ? Role.User : Role.Admin;
            var newUser = await _context.AddAsync(new User() { UserName = username, Role = role });
            await _context.SaveChangesAsync();
            return newUser.Entity;
        }

        public async Task<bool> ContainsUsername(string username) => await _context.Users.AnyAsync(user => user.UserName == username);

        public async Task<User> TryGet(Guid apiKey) => await _context.Users.FirstOrDefaultAsync(user => user.ApiKey == apiKey);

        public async Task<bool> Remove(Guid apiKey)
        {
            var first = await _context.Users.FirstOrDefaultAsync(user => user.ApiKey == apiKey);
            if (first != null)
            {
                _context.Users.Remove(first);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> ChangeRole(string username, string role)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
            if (user != null)
            {
                user.Role = role;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}