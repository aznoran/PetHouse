namespace PetHouse.Accounts.Infrastructure.Options;

public class AdminOptions
{
    public const string ADMIN = "ADMIN";
    public string Email { get; init; }
    public string Username { get; init; }
    public string Password { get; init; }
}