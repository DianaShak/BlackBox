using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.Application;
using UserService.Application.Use_Cases.RegisterUser;
using UserService.Infrastructure;
using UserService.Infrastructure.Context;

namespace UserService;

class Program
{
    static async Task Main(string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        
        var services = new ServiceCollection();
        services.AddApplication();
        services.AddInfrastructure(configuration);
        
        var serviceProvider = services.BuildServiceProvider();
        
        using (var scope = serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                
            Console.WriteLine("Применение миграций.");
            await context.Database.MigrateAsync();
            Console.WriteLine("База данных инициализирована");
            
            var registerHandler = scope.ServiceProvider.GetRequiredService<RegisterUserCommandHandler>();

            while (true)
            {
                Console.WriteLine("Регистрация нового пользователя");
                
                Console.WriteLine("Введите логин:");
                var login = Console.ReadLine();
                
                Console.WriteLine("Введите юзернейм:");
                var username = Console.ReadLine();
                
                Console.WriteLine("Введите пароль:");
                var password = Console.ReadLine();
                
                var command = new RegisterUserCommand(login ?? string.Empty, password ?? string.Empty, username ?? string.Empty);

                try
                {
                    await  registerHandler.Handle(command, CancellationToken.None);
                    Console.WriteLine("Пользователь сохранен в базу данных\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при регистрации: {ex.InnerException?.Message ?? ex.Message}\n");
                }
                
                Console.WriteLine("Зарегистрировать другого пользователя? (y/n): ");
                if (Console.ReadLine()?.ToLower() != "y")
                {
                    break;
                }
            }
        }
        
        Console.WriteLine("Микросервис в фоновом режиме ожидания");
        await Task.Delay(Timeout.Infinite);
    }
}