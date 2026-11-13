using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design; 

namespace UserService.Infrastructure.Context;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>(); // Пишем строку подключения только для создания миграций в терминале
        optionsBuilder.UseNpgsql("Host=localhost;Database=user_service_db;Username=postgres;Password=your_password");
        return new ApplicationDbContext(optionsBuilder.Options);
    }
} 

