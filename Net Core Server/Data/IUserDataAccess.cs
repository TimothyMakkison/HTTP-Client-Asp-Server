using Net_Core_Server.Models;
using System;
using System.Threading.Tasks;

namespace Net_Core_Server.Data
{
    public interface IUserDataAccess
    {
        Task<User> Add(string username);
        Task<bool> ChangeRole(string username, string role);
        Task<bool> ContainsUsername(string username);
        Task<bool> Remove(Guid apiKey);
        Task<User> TryGet(Guid apiKey);
    }
}