using PetHouse.Accounts.Domain.Models;

namespace PetHouse.Accounts.Infrastructure.Options;

public class RolePermissionOptions
{
    public Dictionary<string , string[]> Roles { get; set; }
    public Dictionary<string , string[]> Permissions { get; set; }
}