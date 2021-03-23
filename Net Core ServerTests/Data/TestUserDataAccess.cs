using Net_Core_Server.Data;
using Net_Core_Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Net_Core_ServerTests.Data
{
    public class TestUserDataAccess : IUserDataAccess
    {
        private List<User> _list;

        public TestUserDataAccess()
        {
            _list = new List<User>();
        }

        public void SetDataContext(List<User> context)
        {
            _list = context;
        }

        public Task<User> Add(string username)
        {
            var user = new User()
            {
                UserName = username,
                ApiKey = Guid.NewGuid(),
                Role = Role.User,
            };
            _list.Add(user);
            return Task.FromResult(user);
        }

        public Task<bool> ChangeRole(string username, string role)
        {
            var user = _list.FirstOrDefault(x => x.UserName == username);
            if (user is null)
            {
                return Task.FromResult(false);
            }
            user.Role = role;
            return Task.FromResult(true);
        }

        public Task<bool> ContainsUsername(string username)
        {
            return Task.FromResult(_list.Any(u => u.UserName == username));
        }

        public Task<bool> Remove(Guid apiKey)
        {
            var user = _list.FirstOrDefault(x => x.ApiKey == apiKey);
            if (user is null)
            {
                return Task.FromResult(false);
            }
            _list.Remove(user);
            return Task.FromResult(true);
        }

        public Task<User> TryGet(Guid apiKey)
        {
            return Task.FromResult(_list.FirstOrDefault(x => x.ApiKey == apiKey));
        }
    }
}