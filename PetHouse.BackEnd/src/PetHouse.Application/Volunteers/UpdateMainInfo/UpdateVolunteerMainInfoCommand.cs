using PetHouse.Application.Abstraction;
using PetHouse.Application.Dto;

namespace PetHouse.Application.Volunteers.UpdateMainInfo;

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