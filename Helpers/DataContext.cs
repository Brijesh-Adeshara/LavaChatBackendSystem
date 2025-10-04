using LavaChatBackend.Entities;
using Microsoft.EntityFrameworkCore;

namespace LavaChatBackend.Helpers
{
    public class DataContext : DbContext
    {
        public DbSet<Account> Accounts => Set<Account>();

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                // Use SQL Server or InMemory for testing
                options.UseSqlServer("name=DefaultConnection");
            }
        }
    }
}
