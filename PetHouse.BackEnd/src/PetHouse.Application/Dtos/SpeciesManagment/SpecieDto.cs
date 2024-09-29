namespace PetHouse.Application.Dtos.SpeciesManagment;

public class SpecieDto
{
    public Guid Id { get; init; }
    
    public string Name { get; init; }
    
    public BreedDto[] Breeds { get; init; }
}