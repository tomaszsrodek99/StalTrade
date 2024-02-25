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
        public DbSet<Expense>? Expenses { get; set; }
        public DbSet<PaymentMethod>? PaymentMethods { get; set; }
        public DbSet<Deposit>? Deposit { get; set; }
        public DbSet<StockStatus>? StockStatuses { get; set; }
        public DbSet<Price>? Prices { get; set; }
        public DbSet<Invoice>? Invoices { get; set; }
        public DbSet<InvoiceProduct>? InvoiceProducts { get; set; }
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

            modelBuilder.Entity<Product>()
                .HasOne(p => p.StockStatus)
                .WithOne(ph => ph.Product)
                .HasForeignKey<StockStatus>(ss => ss.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Price>()
                .HasOne(pr => pr.Product)
                .WithMany(p => p.Prices)
                .HasForeignKey(pr => pr.ProductId);

            modelBuilder.Entity<Price>()
                .HasOne(pr => pr.Company)
                .WithMany()
                .HasForeignKey(pr => pr.CompanyId);

            modelBuilder.Entity<Invoice>()
                .HasMany(p => p.ProductsList)
                .WithOne(ph => ph.Invoice)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<InvoiceProduct>()
                .HasOne(ip => ip.Product)
                .WithMany()
                .HasForeignKey(ip => ip.ProductId);

            modelBuilder.Entity<InvoiceProduct>()
                .HasOne(ip => ip.Invoice)
                .WithMany(i => i.ProductsList)
                .HasForeignKey(ip => ip.InvoiceId);
        }

        public override int SaveChanges()
        {
            ApplyCustomLogic();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyCustomLogic();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyCustomLogic()
        {
            var entries = ChangeTracker.Entries<StockStatus>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Modified)
                {
                    var stockStatus = entry.Entity;

                    stockStatus.MarginValue = stockStatus.SoldValue - stockStatus.PurchasedValue;
                    if(stockStatus.MarginValue > 0)
                        stockStatus.Margin = stockStatus.PurchasedValue != 0 ? (stockStatus.MarginValue / stockStatus.PurchasedValue) * 100 : 0;
                    else
                        stockStatus.Margin = 0;
                }
            }
        }
    }
}
