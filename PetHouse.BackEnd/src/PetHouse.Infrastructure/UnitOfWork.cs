﻿using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using PetHouse.Domain.Shared.Other;
using PetHouse.Infrastructure.Data;

namespace PetHouse.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly PetHouseWriteDbContext _writeDbContext;

    public UnitOfWork(PetHouseWriteDbContext writeDbContext)
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
        await _writeDbContext.SaveChangesAsync(cancellationToken);
    }
}