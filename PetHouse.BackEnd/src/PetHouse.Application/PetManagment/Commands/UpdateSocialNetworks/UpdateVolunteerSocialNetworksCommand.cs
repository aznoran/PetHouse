using PetHouse.Application.Abstraction;
using PetHouse.Application.Dtos.PetManagment;

namespace PetHouse.Application.Volunteers.Commands.UpdateSocialNetworks;

public record UpdateVolunteerSocialNetworksCommand(Guid Id, IEnumerable<SocialNetworksDto> SocialNetworksDtos) : ICommand;