namespace PetHouse.Domain;

public record PetIdentifier
{
    public Guid SpeciesId { get; }
    
    public Guid BreedId { get; }
}