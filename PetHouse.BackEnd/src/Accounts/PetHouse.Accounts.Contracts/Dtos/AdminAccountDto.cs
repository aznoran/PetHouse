namespace PetHouse.Accounts.Contracts.Dtos;

public class AdminAccountDto
{
    public Guid Id { get; init; }
    
    public Guid UserId { get; init; }
    
    public string Name { get; init; }
    public string Surname { get; init; }
}