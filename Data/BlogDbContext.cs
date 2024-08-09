using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data;

public class BlogDbContext : DbContext
{
    public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options) { }
    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Blog> Blogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().Property(u => u.CreatedAt).HasDefaultValueSql("GetDate()").ValueGeneratedOnAdd();
        modelBuilder.Entity<User>().Property(u => u.UpdatedAt).HasDefaultValueSql("GetDate()").ValueGeneratedOnAddOrUpdate();
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();

        modelBuilder.Entity<RefreshToken>().Property(r => r.CreatedAt).HasDefaultValueSql("GetDate()").ValueGeneratedOnAdd();

        modelBuilder.Entity<Blog>().Property(b => b.CreatedAt).HasDefaultValueSql("GetDate()").ValueGeneratedOnAdd();
        modelBuilder.Entity<Blog>().Property(b => b.UpdatedAt).HasDefaultValueSql("GetDate()").ValueGeneratedOnAddOrUpdate();

    }
}