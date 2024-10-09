using PetHouse.Core.Dtos.PetManagment;

namespace PetHouse.PetManagement.Contracts.Volunteers.Requests;

public record UpdateVolunteerMainInfoRequest(
    FullNameDto FullNameDto,
    string Email,
    string Description,
    int YearsOfExperience,
    string PhoneNumber
);