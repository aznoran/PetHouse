using PetHouse.Domain.Shared;

namespace PetHouse.Domain.Models;

public sealed class Species : Entity<SpeciesId>
{
    public string Name { get; private set; }

    public ICollection<Breed> Breeds { get; private set; }
}