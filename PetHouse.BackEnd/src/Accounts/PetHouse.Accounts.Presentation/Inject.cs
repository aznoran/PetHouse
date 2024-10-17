using Microsoft.Extensions.DependencyInjection;
using PetHouse.Accounts.Contracts;

namespace PetHouse.Accounts.Presentation;

public static class Inject
{
    public static IServiceCollection AddAccountsPresentation(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IAccountsContract, AccountsContract>();
        
        return serviceCollection;
    }
}