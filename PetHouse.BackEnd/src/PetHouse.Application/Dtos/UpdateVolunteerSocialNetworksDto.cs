using PetHouse.Application.Dto;

namespace PetHouse.Application.Volunteers.UpdateSocialNetworks;

public record UpdateVolunteerSocialNetworksDto(IEnumerable<SocialNetworksDto> SocialNetworksDtos);