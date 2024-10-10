using PetHouse.Core.Dtos.PetManagment;

namespace PetHouse.PetManagement.Contracts.Volunteers.Requests;

public record CreateVolunteerRequest(
    FullNameDto FullNameDto,
    string Description, 
    string Email,
    int YearsOfExperience, 
    string PhoneNumber,
    IEnumerable<SocialNetworksDto> SocialNetworksDto,
    IEnumerable<RequisiteDto> RequisiteDto);