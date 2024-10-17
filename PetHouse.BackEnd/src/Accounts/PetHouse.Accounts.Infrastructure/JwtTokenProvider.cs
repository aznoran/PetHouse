using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PetHouse.Accounts.Application;
using PetHouse.Accounts.Domain;
using PetHouse.Accounts.Domain.Models;
using PetHouse.Accounts.Infrastructure.Options;
using PetHouse.Core.Models;
using PetHouse.SharedKernel.Constraints;

namespace PetHouse.Accounts.Infrastructure;

public class JwtTokenProvider : ITokenProvider
{
    private readonly JwtOptions _jwtOptions;

    public JwtTokenProvider(IOptions<JwtOptions> jwtOptions)
    {
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
}