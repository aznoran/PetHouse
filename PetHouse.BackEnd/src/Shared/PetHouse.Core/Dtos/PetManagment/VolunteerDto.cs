namespace PetHouse.Core.Dtos.PetManagment;

public class VolunteerDto
{
    public bool IsDeleted { get; init; }
    public Guid Id { get; init; }
    public FullNameDto FullName { get; init; }
    public string Description { get; init; }
    public string Email { get; init; }
    public int YearsOfExperience { get; init; }
    public string PhoneNumber { get; init; }
    public IEnumerable<SocialNetworksDto> SocialNetworks { get; init; }
    public IEnumerable<RequisiteDto> Requisites { get; init; }
    public PetDto[] Pets { get; init; }
}