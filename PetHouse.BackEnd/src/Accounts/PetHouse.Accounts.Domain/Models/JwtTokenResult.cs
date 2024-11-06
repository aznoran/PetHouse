namespace PetHouse.Accounts.Domain.Models;

public record JwtTokenResult(string AccessToken, Guid RefreshToken);