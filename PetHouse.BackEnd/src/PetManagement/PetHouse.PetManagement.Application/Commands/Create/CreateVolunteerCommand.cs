using PetHouse.Core.Abstraction;
using PetHouse.Core.Dtos.PetManagment;

namespace PetHouse.PetManagement.Application.Commands.Create;

public record CreateVolunteerCommand(
    FullNameDto FullNameDto,
    string Description, 
    string Email,
    int YearsOfExperience, 
    string PhoneNumber,
    IEnumerable<SocialNetworksDto> SocialNetworksDto,
    IEnumerable<RequisiteDto> RequisiteDto) : ICommand;