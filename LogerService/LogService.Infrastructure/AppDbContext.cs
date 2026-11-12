using System;
using LogerService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace LogService.Infrastructure;

public class AppDbContext : DbContext
{
    public DbSet<LogItem> LogItems { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) {}
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LogItem>()
            .Property(b => b.Metadata)
            .HasColumnType("jsonb");
    }
}
