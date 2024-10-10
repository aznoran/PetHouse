using PetHouse.Core.Abstraction;
using PetHouse.Core.Dtos.PetManagment;

namespace PetHouse.PetManagement.Application.Commands.UpdateMainInfo;

public record UpdateVolunteerMainInfoCommand(
    Guid Id,
    UpdateVolunteerMainInfoDto UpdateVolunteerMainInfoDto
) : ICommand;

public record UpdateVolunteerMainInfoDto(
    FullNameDto FullNameDto,
    string Email,
    string Description,
    int YearsOfExperience,
    string PhoneNumber
);