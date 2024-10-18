using System.Data;
using System.Data.Common;

namespace PetHouse.Accounts.Application;

public interface IUnitOfWork
{
    Task<IDbTransaction> BeginTransaction(CancellationToken cancellationToken = default);
    Task SaveChanges(CancellationToken cancellationToken = default);
}