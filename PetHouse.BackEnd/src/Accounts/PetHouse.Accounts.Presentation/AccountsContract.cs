using PetHouse.Accounts.Contracts;
using PetHouse.Accounts.Infrastructure.Managers;

namespace PetHouse.Accounts.Presentation;

public class AccountsContract : IAccountsContract
{
    private readonly PermissionManager _permissionManager;

    public AccountsContract(PermissionManager permissionManager)
    {
        _permissionManager = permissionManager;
    }
    public async Task<bool> CheckUserPermission(Guid userId, string permissionCode)
    {
        return await _permissionManager.CheckUserPermission(userId, permissionCode);
    }
}