using Microsoft.AspNetCore.Identity;

namespace PetHouse.Accounts.Domain.Models;

public class Role : IdentityRole<Guid>
{
    public List<RolePermission> RolePermissions { get; set; } = [];

    public List<User> Users { get; set; } = [];

}