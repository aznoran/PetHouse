using Microsoft.Extensions.DependencyInjection;

namespace PetHouse.Accounts.Infrastructure.Seeding;

public class AdminAccountsSeeder
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public AdminAccountsSeeder(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task SeedAsync()
    {
        var service = _serviceScopeFactory.CreateScope();

        var scope = service.ServiceProvider.GetRequiredService<AdminAccountsSeederService>();
        
        await scope.SeedAsync();
    }
}