using Microsoft.EntityFrameworkCore;

namespace WorkerService1.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Stock> Stocks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Stock>().UseXminAsConcurrencyToken();
        base.OnModelCreating(modelBuilder);
    }
}