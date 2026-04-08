using Microsoft.EntityFrameworkCore;
using PosApi.Models;

namespace PosApi.Data;

public class PosDbContext : DbContext
{
    public PosDbContext(DbContextOptions<PosDbContext> options) : base(options)
    {
    }

    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Promotion> Promotions { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<CashbackRequest> CashbackRequests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Transaction configuration
        modelBuilder.Entity<Transaction>()
            .HasKey(t => t.Id);
        modelBuilder.Entity<Transaction>()
            .Property(t => t.TransactionId)
            .IsRequired()
            .HasMaxLength(50);
        modelBuilder.Entity<Transaction>()
            .Property(t => t.Amount)
            .HasPrecision(18, 2);

        // Promotion configuration
        modelBuilder.Entity<Promotion>()
            .HasKey(p => p.Id);
        modelBuilder.Entity<Promotion>()
            .Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);
        modelBuilder.Entity<Promotion>()
            .Property(p => p.MinAmount)
            .HasPrecision(18, 2);
        modelBuilder.Entity<Promotion>()
            .Property(p => p.DiscountAmount)
            .HasPrecision(18, 2);

        // Wallet configuration
        modelBuilder.Entity<Wallet>()
            .HasKey(w => w.Id);
        modelBuilder.Entity<Wallet>()
            .Property(w => w.CustomerId)
            .IsRequired()
            .HasMaxLength(50);
        modelBuilder.Entity<Wallet>()
            .Property(w => w.Balance)
            .HasPrecision(18, 2);

        // CashbackRequest configuration
        modelBuilder.Entity<CashbackRequest>()
            .HasKey(c => c.Id);
        modelBuilder.Entity<CashbackRequest>()
            .Property(c => c.TransactionId)
            .IsRequired()
            .HasMaxLength(50);
        modelBuilder.Entity<CashbackRequest>()
            .Property(c => c.Amount)
            .HasPrecision(18, 2);

        // Seed data
        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        // Seed promotions
        modelBuilder.Entity<Promotion>().HasData(
            new Promotion { Id = 1, Name = "10% Off", MinAmount = 50, DiscountAmount = 5, IsActive = true },
            new Promotion { Id = 2, Name = "15% Off", MinAmount = 100, DiscountAmount = 15, IsActive = true },
            new Promotion { Id = 3, Name = "Seasonal Sale", MinAmount = 75, DiscountAmount = 10, IsActive = true }
        );

        // Seed customer wallets
        modelBuilder.Entity<Wallet>().HasData(
            new Wallet { Id = 1, CustomerId = "CUST001", Balance = 500 },
            new Wallet { Id = 2, CustomerId = "CUST002", Balance = 1000 }
        );
    }
}
