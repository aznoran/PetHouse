
using PetHouse.Core.Dtos.PetManagment;

namespace PetHouse.Accounts.Contracts.Dtos;

public class UserDto
{
    public Guid Id { get; init; }

    public string UserName { get; init; } = default!;

    public List<RoleDto> Roles { get; init; } = [];

    public IEnumerable<SocialNetworksDto>? SocialNetworks { get; init; } = [];
    
    public AdminAccountDto? AdminAccount { get; set; }
    
    public ParticipantAccountDto? ParticipantAccount { get; set; }
    
    public VolunteerAccountDto? VolunteerAccount { get; set; }
}