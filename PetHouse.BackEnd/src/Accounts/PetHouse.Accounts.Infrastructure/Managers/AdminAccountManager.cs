using Microsoft.EntityFrameworkCore;
using PetHouse.Accounts.Domain.Models;
using PetHouse.Accounts.Infrastructure.Data;

namespace PetHouse.Accounts.Infrastructure.Managers;

public class AdminAccountManager
{
    private readonly AccountsDbContext _dbContext;

    public AdminAccountManager(AccountsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAdminAccount(AdminAccount adminAccount)
    {
        if (await _dbContext.AdminAccounts
                .FirstOrDefaultAsync(ac => ac.FullName == adminAccount.FullName) is not null)
            return;
        
        await _dbContext.AdminAccounts.AddAsync(adminAccount);
    }
}