using Domain.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.EntityFramework;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    /// <summary>
    /// Чаты.
    /// </summary>
    public DbSet<Chat> Chats { get; set; }

    /// <summary>
    /// Участник чата.
    /// </summary>
    public DbSet<ChatMember> ChatMembers { get; set; }

    /// <summary>
    /// Сообщения.
    /// </summary>
    public DbSet<Message> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Chat>()
            .HasMany(u => u.Messages);
            //.WithOne(c => c.Course)
            //.IsRequired();

        //modelBuilder.Entity<Course>().Property(c => c.Name).HasMaxLength(100);
        //modelBuilder.Entity<Lesson>().Property(c => c.Subject).HasMaxLength(100);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
    }
}
