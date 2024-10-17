using Microsoft.EntityFrameworkCore;
using PetHouse.Accounts.Domain.Models;
using PetHouse.Accounts.Infrastructure.Data;

namespace PetHouse.Accounts.Infrastructure.Managers;

public class PermissionManager
{
    private readonly AccountsDbContext _dbContext;

    public PermissionManager(AccountsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddPermissionIfNotExists(string permission)
    {
        var res = await _dbContext.Permissions
            .AnyAsync(p => p.Code == permission);

        if (!res)
            await _dbContext.Permissions.AddAsync(new Permission() { Code = permission });
    }

    public async Task<bool> CheckUserPermission(Guid userId, string permissionCode)
    {
        var roleNames = await _dbContext.Users.Include(user => user.Roles)
            .Where(u => u.Id == userId)
            .SelectMany(u => u.Roles)
            .Select(r => r.Name)
            .ToListAsync();

        if (roleNames.Count == 0)
            return false;

        return await _dbContext.RolePermissions.Where(rp => roleNames.Contains(rp.Role.Name))
            .FirstOrDefaultAsync(rp => rp.Permission.Code == permissionCode) is not null;
    }

}