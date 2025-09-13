using KanbanTaskBoard.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace KanbanTaskBoard.Api.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskItem>().HasKey(t => t.Id);
        modelBuilder.Entity<TaskItem>().Property(t => t.Title).IsRequired().HasMaxLength(140);
        modelBuilder.Entity<TaskItem>().Property(t => t.Status).HasConversion<int>();
    }
}

