namespace PetHouse.Application.Volunteers.CreateVolunteer;

public record CreateVolunteerRequest(
    FullNameDto FullNameDto,
    string Description, 
    int YearsOfExperience, 
    string PhoneNumber,
    IEnumerable<SocialNetworksDto> SocialNetworksDto,
    IEnumerable<RequisiteDto> RequisiteDto);