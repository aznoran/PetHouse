namespace PetHouse.Domain;

public class Pet
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string Specie { get; set; }
    
    public string Description { get; set; }
    
    public string Breed { get; set; }
    
    public string Color { get; set; }
    
    public string HealthDescription { get; set; }
    
    public string Location { get; set; }
    
    public double Weight { get; set; }
    
    public double Height { get; set; }
    
    public string PhoneNumber { get; set; }
    
    public bool IsCastrated { get; set; }
    
    public DateTime BirthdayDate { get; set; }
    
    public bool IsVaccinated { get; set; }
    
    public byte PetStatus { get; set; }
    
    public Requisite Requisites { get; set; }
    
    public DateTime CreationDate { get; set; }
}