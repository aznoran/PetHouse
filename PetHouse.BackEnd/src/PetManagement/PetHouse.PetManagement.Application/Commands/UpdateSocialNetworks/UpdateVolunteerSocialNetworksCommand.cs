using PetHouse.Core.Abstraction;
using PetHouse.Core.Dtos.PetManagment;

namespace PetHouse.PetManagement.Application.Commands.UpdateSocialNetworks;

public record UpdateVolunteerSocialNetworksCommand(Guid Id, IEnumerable<SocialNetworksDto> SocialNetworksDtos) : ICommand;