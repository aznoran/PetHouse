using PetHouse.Application.Abstraction;
using PetHouse.Application.Dtos.PetManagment;

namespace PetHouse.Application.Volunteers.Commands.UpdateMainInfo;

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