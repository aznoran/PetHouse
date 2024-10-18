using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using PetHouse.PetManagement.Application;
using PetHouse.PetManagement.Domain.Entities;
using PetHouse.PetManagement.Infrastructure.Data;

namespace PetHouse.PetManagement.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly WriteDbContext _writeDbContext;

    public UnitOfWork(WriteDbContext writeDbContext)
    {
        _writeDbContext = writeDbContext;
    }

    public async Task<IDbTransaction> BeginTransaction(CancellationToken cancellationToken = default)
    {
        var transaction = await _writeDbContext.Database.BeginTransactionAsync(cancellationToken);

        return transaction.GetDbTransaction();
    }

    public async Task SaveChanges(CancellationToken cancellationToken = default)
    {
        var a = _writeDbContext.ChangeTracker.Entries<Pet>();
        await _writeDbContext.SaveChangesAsync(cancellationToken);
    }
}