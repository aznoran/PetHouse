namespace PetHouse.Application.Volunteers.CreateVolunteer;

public record CreateVolunteerRequest(
    VolunteerProfileDto VolunteerProfileDto,
    IEnumerable<SocialNetworksDto> SocialNetworksDto,
    IEnumerable<RequisiteDto> RequisiteDto);