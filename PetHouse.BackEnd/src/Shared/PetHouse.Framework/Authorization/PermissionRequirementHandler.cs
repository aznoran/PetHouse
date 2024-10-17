using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using PetHouse.Accounts.Contracts;
using PetHouse.SharedKernel.Constraints;

namespace PetHouse.Framework.Authorization;

public class PermissionRequirementHandler : AuthorizationHandler<PermissionAttribute>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public PermissionRequirementHandler(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, PermissionAttribute requirement)
    {
        var userId = context.User.FindFirst(
            c => c.Type == CustomClaims.Id);

        if (userId == null)
            return;
        
        var scope = _serviceScopeFactory.CreateScope();

        var accountsContract = scope.ServiceProvider.GetRequiredService<IAccountsContract>();

        var isAccessAvailable = await accountsContract.CheckUserPermission(Guid.Parse(userId.Value), requirement.Code);

        if(isAccessAvailable)
            context.Succeed(requirement);

        return;
    }
}