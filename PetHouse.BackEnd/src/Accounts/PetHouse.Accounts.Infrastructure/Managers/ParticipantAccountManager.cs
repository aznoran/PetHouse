using PetHouse.Accounts.Domain.Models;
using PetHouse.Accounts.Infrastructure.Data;

namespace PetHouse.Accounts.Infrastructure.Managers;

public class ParticipantAccountManager : IParticipantAccountManager
{
    private readonly AccountsDbContext _dbContext;

    public ParticipantAccountManager(AccountsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddParticipantAccount(ParticipantAccount participantAccount)
    {
        await _dbContext.ParticipantAccounts.AddAsync(participantAccount);
    }
}