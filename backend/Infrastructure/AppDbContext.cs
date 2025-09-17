using System;
using KanbanTaskBoard.Api.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KanbanTaskBoard.Api.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var dtoToLong = new ValueConverter<DateTimeOffset, long>(
            to  => to.ToUnixTimeMilliseconds(),
            from => DateTimeOffset.FromUnixTimeMilliseconds(from)
        );

        modelBuilder.Entity<TaskItem>(e =>
        {
            e.HasKey(t => t.Id);
            e.Property(t => t.Title).IsRequired().HasMaxLength(140);
            e.Property(t => t.Status).HasConversion<int>();

            // kritik: DateTimeOffset <-> long (INTEGER) dönüşümü
            e.Property(t => t.CreatedAt)
                .HasConversion(dtoToLong)
                .HasColumnType("INTEGER");

            e.Property(t => t.UpdatedAt)
                .HasConversion(dtoToLong)
                .HasColumnType("INTEGER");
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=kanban.db");
        }
    }
}
