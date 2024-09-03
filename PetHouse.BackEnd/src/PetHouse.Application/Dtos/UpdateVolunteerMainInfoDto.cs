using PetHouse.Application.Dto;

namespace PetHouse.Application.Volunteers.UpdateMainInfo;

public record UpdateVolunteerMainInfoDto(
    FullNameDto FullNameDto,
    string Email,
    string Description,
    int YearsOfExperience,
    string PhoneNumber
);