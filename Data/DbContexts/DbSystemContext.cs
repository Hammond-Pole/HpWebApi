using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.DbContexts;

public class DbSystemContext : DbContext
{
    public DbSystemContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User>? Users { get; set; }
    public DbSet<Selection> Selections { get; set; }
    public DbSet<Question> Questions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure your model here
        modelBuilder.Entity<Selection>()
            .HasMany(s => s.Questions)
            .WithOne(q => q.Selection);

        base.OnModelCreating(modelBuilder);
    }
}