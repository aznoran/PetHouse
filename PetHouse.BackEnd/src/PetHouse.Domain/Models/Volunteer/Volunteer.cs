using PetHouse.Domain.ValueObjects;

namespace PetHouse.Domain;

public class Volunteer : Entity<VolunteerId>
{
    
    public string FullName { get; private set; }
    
    public string Description { get; private set; }
    
    public int YearsOfExperience { get; private set; }
    
    public int CountOfPetsFoundHome { get; private set; }
    
    public int CountOfPetsLookingForHome { get; private set; }
    
    public int CountOfPetsOnTreatment { get; private set; }
    
    public string PhoneNumber { get; private set; }
    
    public SocialNetworkInfo SocialNetworks { get; private set; }
    
    public RequisiteInfo Requisites { get; private set; }
    
    public ICollection<Pet> Pets { get; private set; }
    
}