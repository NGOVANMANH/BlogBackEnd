using api.Enities;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace api.Data;

public class MongoContext : DbContext
{
    public MongoContext(DbContextOptions options) : base(options)
    { }

    public DbSet<Message> Messages { get; init; }
    public DbSet<OTPManager> OTPManagers { get; init; }
    public DbSet<Room> Rooms { get; init; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Room>().HasIndex(r => r.CreatedAt);
    }
}