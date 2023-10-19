using Microsoft.EntityFrameworkCore;
using StalTradeApi.Models;

namespace StalTradeApi.Context
{
    public class StalTradeDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public StalTradeDbContext(DbContextOptions options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=StalTrade;Trusted_Connection=True;",
                x => x.MigrationsHistoryTable("__EFMigrationsHistory", "Identity"));
        }
    }

}
