namespace PetHouse.Domain;

public class Volunteer
{
    public Guid Id { get; private set; }
    
    public string FullName { get; private set; }
    
    public string Description { get; private set; }
    
    public short YearsOfExperience { get; private set; }
    
    public int CountOfPetsFoundHome { get; private set; }
    
    public int CountOfPetsLookingForHome { get; private set; }
    
    public int CountOfPetsOnTreatment { get; private set; }
    
    public string PhoneNumber { get; private set; }
    
    public ICollection<SocialNetwork> SocialNetworks { get; private set; }
    
    public ICollection<Requisite> Requisites { get; private set; }
    
    public ICollection<Pet> Pets { get; private set; }
    
}