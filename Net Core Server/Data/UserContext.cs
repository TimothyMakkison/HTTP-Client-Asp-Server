using Microsoft.EntityFrameworkCore;
using Net_Core_Server.Models;
using System.Diagnostics.CodeAnalysis;

namespace Net_Core_Server.Data
{
    public class UserContext : DbContext
    {
        public UserContext([NotNullAttribute] DbContextOptions<UserContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}