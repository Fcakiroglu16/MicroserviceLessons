using Microsoft.EntityFrameworkCore;

namespace WorkerServiceWithSubscriber.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<QueueIdempotentInbox> QueueIdempotentInboxes { get; set; }
}