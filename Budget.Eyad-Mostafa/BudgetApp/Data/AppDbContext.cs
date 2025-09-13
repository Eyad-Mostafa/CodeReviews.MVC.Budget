using BudgetApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetApp.Data;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>()
            .HasMany(c => c.Transactions)
            .WithOne(t => t.Category)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Transaction>()
            .Property(t => t.Type)
            .HasConversion<string>();
    }
}
