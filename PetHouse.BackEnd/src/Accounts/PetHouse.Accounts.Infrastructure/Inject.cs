using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetHouse.Accounts.Application;
using PetHouse.Accounts.Domain.Models;
using PetHouse.Accounts.Infrastructure.Data;
using PetHouse.Accounts.Infrastructure.Managers;
using PetHouse.Accounts.Infrastructure.Options;
using PetHouse.Accounts.Infrastructure.Seeding;
using PetHouse.Core.Providers;
using PetHouse.SharedKernel.Constraints;

namespace PetHouse.Accounts.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddAccountsInfrastructure(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddTransient<ITokenProvider, JwtTokenProvider>();
        
        serviceCollection.Configure<JwtOptions>(configuration.GetSection(JwtOptions.JWT));
        serviceCollection.Configure<AdminOptions>(configuration.GetSection( AdminOptions.ADMIN));
        
        serviceCollection.AddScoped<AccountsDbContext>();

        serviceCollection.AddKeyedScoped<IUnitOfWork, UnitOfWork>(ModuleNames.Accounts);

        serviceCollection.AddIdentity<User, Role>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireDigit = true;
            })
            .AddEntityFrameworkStores<AccountsDbContext>()
            .AddDefaultTokenProviders();

        serviceCollection.AddScoped<UserManager<User>>();
        serviceCollection.AddScoped<RoleManager<Role>>();
        
        serviceCollection.AddScoped<PermissionManager>();
        serviceCollection.AddScoped<RolePermissionManager>();
        serviceCollection.AddScoped<IAccountManager, AccountManager>();
        
        serviceCollection.AddSingleton<AdminAccountsSeeder>();
        
        serviceCollection.AddScoped<AdminAccountsSeederService>();
        
        return serviceCollection;
    }
}