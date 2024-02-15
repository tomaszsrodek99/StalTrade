using Microsoft.EntityFrameworkCore;
using StalTradeAPI.Models;

namespace StalTradeAPI.Context
{
    public class StalTradeDbContext : DbContext
    {
        public DbSet<User>? Users { get; set; }
        public DbSet<Contact>? Contacts { get; set; }
        public DbSet<Company>? Companies { get; set; }
        public DbSet<Product>? Products { get; set; }
        public DbSet<PriceHistory>? PriceHistory { get; set; }
        public DbSet<Expense>? Expenses { get; set; }
        public DbSet<PaymentMethod>? PaymentMethods { get; set; }
        public DbSet<Deposit>? Deposit { get; set; }
        public StalTradeDbContext(DbContextOptions options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=StalTrade;Trusted_Connection=True;",
                x => x.MigrationsHistoryTable("__EFMigrationsHistory", "Identity"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>()
                .HasMany(c => c.Contacts)
                .WithOne(e => e.Company)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Expense>()
                .Property(e => e.Date)
                .HasColumnType("date");

            modelBuilder.Entity<Expense>()
                .Property(e => e.DateOfPayment)
                .HasColumnType("date");
        }
    }

}
