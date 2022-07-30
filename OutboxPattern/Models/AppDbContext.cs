using Microsoft.EntityFrameworkCore;

namespace OutboxPattern.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
    {
    }

    public DbSet<Outbox> Outboxes { get; set; }
    public DbSet<Order> Orders { get; set; }
}