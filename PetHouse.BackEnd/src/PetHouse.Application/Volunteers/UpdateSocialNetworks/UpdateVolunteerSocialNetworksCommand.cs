using PetHouse.Application.Abstraction;
using PetHouse.Application.Dto;

namespace PetHouse.Application.Volunteers.UpdateSocialNetworks;

public record UpdateVolunteerSocialNetworksCommand(Guid Id, IEnumerable<SocialNetworksDto> SocialNetworksDtos) : ICommand;