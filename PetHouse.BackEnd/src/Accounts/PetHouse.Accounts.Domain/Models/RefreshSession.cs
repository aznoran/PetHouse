namespace PetHouse.Accounts.Domain.Models;

public class RefreshSession
{
    public Guid Id { get; init; }
    public Guid RefreshToken { get; init; }
    
    public Guid UserId { get; init; }
    
    public User User { get; init; }
    
    public DateTime CreatedAt { get; init; }
    
    public DateTime ExpiresAt { get; init; }
}