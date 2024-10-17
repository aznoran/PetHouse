namespace PetHouse.Accounts.Domain.Models;

public class RolePermission
{
    
    public Role Role { get; set; }
    public Guid RoleId { get; set; }
    public Permission Permission { get; set; }
    public Guid PermissionId { get; set; }
}