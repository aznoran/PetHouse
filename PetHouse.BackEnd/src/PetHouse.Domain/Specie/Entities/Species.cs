using PetHouse.Domain.Shared.Id;
using PetHouse.Domain.Shared.ValueObjects;

namespace PetHouse.Domain.Specie.Entities;

public sealed class Species : Entity<SpeciesId>
{
    public string Name { get; private set; }

    public ICollection<Breed> Breeds { get; private set; }
}