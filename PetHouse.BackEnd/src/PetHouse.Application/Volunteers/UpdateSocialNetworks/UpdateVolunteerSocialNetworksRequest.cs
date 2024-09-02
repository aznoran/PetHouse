using PetHouse.Application.Dto;

namespace PetHouse.Application.Volunteers.UpdateSocialNetworks;

public record UpdateVolunteerSocialNetworksRequest(Guid Id, IEnumerable<SocialNetworksDto> SocialNetworksDtos);