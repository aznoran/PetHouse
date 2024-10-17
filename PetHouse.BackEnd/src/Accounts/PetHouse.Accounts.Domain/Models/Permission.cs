namespace PetHouse.Accounts.Domain.Models;

public class Permission
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    
    private List<Role> _roles = [];
    public IReadOnlyList<Role> Roles => _roles;
}