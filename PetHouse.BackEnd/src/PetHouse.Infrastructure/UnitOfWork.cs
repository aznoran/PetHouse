using System.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace PetHouse.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly PetHouseDbContext _dbContext;

    public UnitOfWork(PetHouseDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IDbTransaction> BeginTransaction(CancellationToken cancellationToken = default)
    {
        var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        return transaction.GetDbTransaction();
    }

    public async Task SaveChanges(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}