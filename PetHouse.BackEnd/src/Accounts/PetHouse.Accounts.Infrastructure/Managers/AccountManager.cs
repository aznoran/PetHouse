﻿using Microsoft.EntityFrameworkCore;
using PetHouse.Accounts.Application;
using PetHouse.Accounts.Domain.Models;
using PetHouse.Accounts.Infrastructure.Data;

namespace PetHouse.Accounts.Infrastructure.Managers;

public class AccountManager : IAccountManager
{
    private readonly AccountsWriteDbContext _dbContext;

    public AccountManager(AccountsWriteDbContext dbContext)
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
    
    public async Task AddParticipantAccount(ParticipantAccount participantAccount)
    {
        await _dbContext.ParticipantAccounts.AddAsync(participantAccount);
    }
}