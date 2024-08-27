namespace PetHouse.Application.Volunteers.CreateVolunteer;

public record CreateVolunteerRequest(VolunteerProfileDto VolunteerProfileDto, ICollection<SocialNetworksDto> SocialNetworksDto, ICollection<RequisiteDto> RequisiteDto);
public record VolunteerProfileDto(string FullName, string Description, int YearsOfExperience, string PhoneNumber);
public record RequisiteDto(string Name, string Description);
public record SocialNetworksDto(string Link, string Name);