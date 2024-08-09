using api.MongoDB.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace api.Data;

public class MongoContext : DbContext
{
    public MongoContext(DbContextOptions options) : base(options)
    { }

    public DbSet<Message> Messages { get; init; }
    public DbSet<OTPManager> OTPManagers { get; init; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Message>().ToCollection("messages");
        modelBuilder.Entity<OTPManager>().ToCollection("OTPManagers");
    }
}