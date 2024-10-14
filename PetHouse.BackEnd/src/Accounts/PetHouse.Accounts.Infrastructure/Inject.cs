using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetHouse.Accounts.Application;
using PetHouse.Accounts.Domain.Models;
using PetHouse.Accounts.Infrastructure.Data;
using PetHouse.Accounts.Infrastructure.Options;

namespace PetHouse.Accounts.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddAccountsInfrastructure(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        var c = configuration.GetSection(JwtOptions.JWT);
     
        serviceCollection.AddTransient<ITokenProvider, JwtTokenProvider>();
        
        serviceCollection.Configure<JwtOptions>(configuration.GetSection(JwtOptions.JWT));
        
        serviceCollection.AddScoped<AccountsDbContext>();

        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();

        serviceCollection.AddIdentity<User, Role>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireDigit = true;
            })
            .AddEntityFrameworkStores<AccountsDbContext>()
            .AddDefaultTokenProviders();

        serviceCollection.AddScoped<UserManager<User>>();
        
        return serviceCollection;
    }
}