using PetHouse.Application.Abstraction;
using PetHouse.Application.Dtos.PetManagment;

namespace PetHouse.Application.PetManagement.Commands.UpdateSocialNetworks;

public record UpdateVolunteerSocialNetworksCommand(Guid Id, IEnumerable<SocialNetworksDto> SocialNetworksDtos) : ICommand;