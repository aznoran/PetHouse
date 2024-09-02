using PetHouse.Application.Dto;

namespace PetHouse.Application.Volunteers.Create;

public record CreateVolunteerRequest(
    FullNameDto FullNameDto,
    string Description, 
    string Email,
    int YearsOfExperience, 
    string PhoneNumber,
    IEnumerable<SocialNetworksDto> SocialNetworksDto,
    IEnumerable<RequisiteDto> RequisiteDto);