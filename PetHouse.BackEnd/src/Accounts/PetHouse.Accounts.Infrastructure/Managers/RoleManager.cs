using Microsoft.EntityFrameworkCore;
using PetHouse.Accounts.Domain.Models;
using PetHouse.Accounts.Infrastructure.Data;

namespace PetHouse.Accounts.Infrastructure.Managers;

public class RoleManager
{
    private readonly AccountsDbContext _dbContext;

    public RoleManager(AccountsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddRoleIfNotExists(string roleName)
    {
        var res = await _dbContext.Roles
            .AnyAsync(p => p.Name == roleName);

        if (!res)
            await _dbContext.Roles.AddAsync(new Role(){Name = roleName});
    }
    
    public async Task<Role?> FindByNameAsync(string roleName)
    {
        return await _dbContext.Roles
            .FirstOrDefaultAsync(p => p.Name == roleName);
    }
}