using Microsoft.EntityFrameworkCore;

namespace WorkerService2.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Stock> Stocks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Stock>().HasData(new Stock { Id = 1, Name = "Pen 1", Count = 100 });

        base.OnModelCreating(modelBuilder);
    }
}