namespace PetHouse.Core.Dtos.SpeciesManagment;

public class BreedDto
{
    public Guid Id { get; init; }
    
    public string Name { get; init; }
    
    public Guid SpecieId { get; init; }
}