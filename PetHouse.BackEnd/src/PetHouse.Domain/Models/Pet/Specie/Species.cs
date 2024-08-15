namespace PetHouse.Domain.Models;

public class Species : Entity<SpeciesId>
{
    public string Name { get; private set; }
    public ICollection<Breed> Breeds { get; private set; }
}