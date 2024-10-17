using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.Accounts.Domain.Models;

public class User : IdentityUser<Guid>
{
    private User() { }

    private User(string userName,
        string email,
        List<Role> roles)
    {
        UserName = userName;
        Email = email;
        _roles = roles;
    }

    private List<SocialNetwork> _socialNetworks = [];
    public IReadOnlyList<SocialNetwork> SocialNetworks => _socialNetworks;
    
    private List<Role> _roles = [];
    public IReadOnlyList<Role> Roles => _roles;

    public static User CreateAdmin(string userName,
        string email,
        Role role)
    {
        return new(userName, email, [role]);
    }
    
    public static User CreateParticipant(string userName,
        string email,
        Role role)
    {
        return new(userName, email, [role]);
    }
    
    public void UpdateSocialNetworks(IEnumerable<SocialNetwork> socialNetworks)
    {
        _socialNetworks = socialNetworks.ToList();
    }
}