using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;
using UserService.Domain.Enums;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Models;

namespace UserService.Infrastructure.Context;

public class ApplicationDbContext : DbContext,  IUnitOfWork
{
    public DbSet<UsersModel> Users { get; set; }
    public DbSet<UserProfilesModel> UserProfiles { get; set; }
    public DbSet<PasswordRecordModel> PasswordRecords { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<UsersModel>()
            .HasOne(u => u.Profile)
            .WithOne(p => p.User)
            .HasForeignKey<UserProfilesModel>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UsersModel>()
            .HasMany(u => u.PasswordHistory)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<UsersModel>()
            .Property(u => u.Status)
            .HasConversion<int>()
            .HasDefaultValue(UserStatusEnum.Offline);
        
        modelBuilder.Entity<UsersModel>()
            .Property(u => u.UserRole)
            .HasConversion<int>()
            .HasDefaultValue(UserRole.Member);
    }
}