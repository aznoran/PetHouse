using PetHouse.Accounts.Domain.Models;

namespace PetHouse.Accounts.Application;

public interface ITokenProvider
{
    string GenerateAccessToken(User user, CancellationToken cancellationToken);

    Task<Guid> GenerateRefreshToken(User user, CancellationToken cancellationToken);
}