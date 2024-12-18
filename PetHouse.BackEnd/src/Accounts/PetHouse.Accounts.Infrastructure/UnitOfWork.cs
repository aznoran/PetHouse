﻿using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PetHouse.Accounts.Application;
using PetHouse.Accounts.Infrastructure.Data;
using PetHouse.Core.Providers;

namespace PetHouse.Accounts.Infrastructure;

internal class UnitOfWork : IUnitOfWork
{
    private readonly AccountsWriteDbContext _writeDbContext;

    public UnitOfWork(AccountsWriteDbContext writeDbContext)
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