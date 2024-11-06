using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PetHouse.Accounts.Application;
using PetHouse.Accounts.Domain.Models;
using PetHouse.Accounts.Infrastructure.Data;
using PetHouse.Accounts.Infrastructure.Options;
using PetHouse.SharedKernel.Constraints;

namespace PetHouse.Accounts.Infrastructure;

public class JwtTokenProvider : ITokenProvider
{
    private readonly AccountsDbContext _dbContext;
    private readonly JwtOptions _jwtOptions;

    public JwtTokenProvider(IOptions<JwtOptions> jwtOptions,
        AccountsDbContext dbContext)
    {
        _dbContext = dbContext;
        _jwtOptions = jwtOptions.Value;
    }

    public string GenerateAccessToken(User user, CancellationToken cancellationToken)
    {
        var signingCredentials =  new SigningCredentials(
            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOptions.Key)), 
            SecurityAlgorithms.HmacSha256);

        Claim[] claims = new[]
        {
            new Claim(CustomClaims.Email, user.Email),
            new Claim(CustomClaims.Id, user.Id.ToString())
        };
        var jwt = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(double.Parse(_jwtOptions.ExpiredMinutesTime)),
            signingCredentials: signingCredentials);
        
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        return encodedJwt;
    }
    
    public async Task<Guid> GenerateRefreshToken(User user, CancellationToken cancellationToken)
    {
        var refreshSession = new RefreshSession()
        {
            RefreshToken = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(30),
            UserId = user.Id
        };

        await _dbContext.RefreshSessions.AddAsync(refreshSession, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return refreshSession.RefreshToken;
    }
}