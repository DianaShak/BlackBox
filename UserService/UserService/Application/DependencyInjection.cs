using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Use_Cases.RegisterUser;

namespace UserService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<RegisterUserCommandHandler>();
        return services;
    }
}