using PetHouse.Domain.Enums;

namespace PetHouse.Domain;

public class Pet : Entity
{
    public string Name { get; private set; }
    
    public string Specie { get; private set; }
    public string Description { get; private set; }
    
    public string Breed { get; private set; }
    
    public string Color { get; private set; }
    
    public string HealthInfo { get; private set; }
    
    public string Address { get; private set; }
    
    public double Weight { get; private set; }
    
    public double Height { get; private set; }
    
    public string PhoneNumber { get; private set; }
    
    public bool IsCastrated { get; private set; }
    
    public DateTime BirthdayDate { get; private set; }
    
    public bool IsVaccinated { get; private set; }
    
    public PetStatus PetStatus { get; private set; }
    
    public ICollection<Requisite> Requisites { get; private set; }
    
    public DateTime CreationDate { get; private set; }
    
    public ICollection<PetPhoto> PetPhotos { get; private set; }
    
    
}