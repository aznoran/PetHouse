using Microsoft.EntityFrameworkCore;
using PetHouse.Accounts.Domain.Models;
using PetHouse.Accounts.Infrastructure.Data;

namespace PetHouse.Accounts.Infrastructure.Managers;

public class RolePermissionManager
{
    private readonly AccountsWriteDbContext _dbContext;

    public RolePermissionManager(AccountsWriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddRolePermissionsIfNotExists(string roleName , IEnumerable<string> permissions)
    {
        var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
        if (role == null)
            throw new ApplicationException($"Role {roleName} is not found.");
        
        foreach (var permissionCode in permissions)
        {
            var permission = await _dbContext.Permissions.FirstOrDefaultAsync(p => p.Code == permissionCode);
            if (permission == null)
                throw new ApplicationException($"Permission code {permissionCode} is not found.");
            
            var rolePermissionExist = await _dbContext.RolePermissions
                .AnyAsync(rp => rp.RoleId == role.Id && rp.PermissionId == permission.Id);

            if (rolePermissionExist)
                continue;

            await _dbContext.RolePermissions.AddAsync(new RolePermission()
                { PermissionId = permission!.Id, RoleId = role.Id });
        }
    }
}