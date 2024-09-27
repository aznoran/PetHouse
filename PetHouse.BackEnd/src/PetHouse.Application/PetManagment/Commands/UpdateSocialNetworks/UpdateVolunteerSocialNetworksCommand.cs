using PetHouse.Application.Abstraction;
using PetHouse.Application.Dtos.PetManagment;

namespace PetHouse.Application.PetManagment.Commands.UpdateSocialNetworks;

public record UpdateVolunteerSocialNetworksCommand(Guid Id, IEnumerable<SocialNetworksDto> SocialNetworksDtos) : ICommand;