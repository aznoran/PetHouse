namespace PetHouse.Application.Volunteers.CreateVolunteer;

public record CreateVolunteerRequest(VolunteerProfileDto VolunteerProfileDto, ICollection<SocialNetworksDto> SocialNetworksDto, ICollection<RequisiteDto> RequisiteDto);
public record VolunteerProfileDto(string FullName, string Description, int YearsOfExperience, int CountOfPetsFoundHome, int CountOfPetsLookingForHome, int CountOfPetsOnTreatment, string PhoneNumber);
public record RequisiteDto(string Name, string Description);
public record SocialNetworksDto(string Reference, string Name);