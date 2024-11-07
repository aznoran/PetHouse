using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using PetHouse.SharedKernel.Other;
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
    
    public AdminAccount? AdminAccount { get; init; }
    public VolunteerAccount? VolunteerAccount { get; init; }
    public ParticipantAccount? ParticipantAccount { get; init; }

    public static Result<User, Error> CreateAdmin(string userName,
        string email,
        Role role)
    {
        if (role.Name != AdminAccount.ADMIN)
            return Errors.Accounts.InvalidRole();
        
        return Result.Success<User, Error>(new(userName, email, [role]));
    }
    
    public static Result<User, Error> CreateParticipant(string userName,
        string email,
        Role role)
    {
        if (role.Name != ParticipantAccount.PARTICIPANT)
            return Errors.Accounts.InvalidRole();
        
        return Result.Success<User, Error>(new(userName, email, [role]));
    }
    
    public void UpdateSocialNetworks(IEnumerable<SocialNetwork> socialNetworks)
    {
        _socialNetworks = socialNetworks.ToList();
    }
}