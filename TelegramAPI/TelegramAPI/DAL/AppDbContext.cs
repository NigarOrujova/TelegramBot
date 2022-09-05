using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TelegramAPI.Models;

namespace TelegramAPI.DAL
{
    public class AppDbContext : DbContext
    {
        private const string connectionString = "Server=;Database=;Integrated Security=sspi;";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
        public DbSet<Message> Messages { get; set; }
    }
}
