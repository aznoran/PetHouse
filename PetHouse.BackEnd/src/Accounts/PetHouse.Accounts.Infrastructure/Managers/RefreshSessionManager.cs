using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetHouse.Accounts.Application;
using PetHouse.Accounts.Domain.Models;
using PetHouse.Accounts.Infrastructure.Data;
using PetHouse.SharedKernel.Other;
using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.Accounts.Infrastructure.Managers;

public class RefreshSessionManager : IRefreshSessionManager
{
     private readonly AccountsWriteDbContext _dbContext;

     public RefreshSessionManager(AccountsWriteDbContext dbContext)
     {
          _dbContext = dbContext;
     }
     
     public async Task<Result<RefreshSession, Error>> GetRefreshSesssionByToken(Guid refreshToken
          , CancellationToken cancellationToken)
     {
          var refreshSession = await _dbContext.RefreshSessions
               .FirstOrDefaultAsync(rs => rs.RefreshToken == refreshToken,cancellationToken);

          if (refreshSession is null)
          {
               return Errors.General.NotFound(refreshToken);
          }

          return refreshSession;
     }
     
     public async Task<Result<RefreshSession, Error>> GetRefreshSesssionByUserId(Guid userId
          , CancellationToken cancellationToken)
     {
          var refreshSession = await _dbContext.RefreshSessions
               .FirstOrDefaultAsync(rs => rs.UserId == userId,cancellationToken);

          if (refreshSession is null)
          {
               return Errors.General.NotFound(userId);
          }

          return refreshSession;
     }
     
     public void DeleteRefreshSession(RefreshSession refreshSession, CancellationToken cancellationToken)
     {
          _dbContext.RefreshSessions.Remove(refreshSession);
     }
}