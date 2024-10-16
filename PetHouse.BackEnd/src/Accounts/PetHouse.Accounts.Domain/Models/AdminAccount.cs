using PetHouse.PetManagement.Domain.ValueObjects;

namespace PetHouse.Accounts.Domain.Models;

public class AdminAccount
{
    public const string ADMIN = "Admin";
    public Guid Id { get; set; }
    public User User { get; set; }
    public Guid UserId { get; set; }
    public FullName FullName { get; set; }
}