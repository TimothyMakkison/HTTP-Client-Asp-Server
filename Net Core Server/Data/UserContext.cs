using Microsoft.EntityFrameworkCore;
using Net_Core_Server.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

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
