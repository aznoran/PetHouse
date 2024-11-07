using CSharpFunctionalExtensions;
using PetHouse.Accounts.Domain.Models;
using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.Accounts.Application;

public interface IRefreshSessionManager
{
    Task<Result<RefreshSession, Error>> GetRefreshSesssionByToken(Guid refreshToken, 
        CancellationToken cancellationToken);

    void DeleteRefreshSession(RefreshSession refreshSession, CancellationToken cancellationToken);

    Task<Result<RefreshSession, Error>> GetRefreshSesssionByUserId(Guid userId
        , CancellationToken cancellationToken);
}