using PetHouse.Domain.Shared.Id;
using PetHouse.Domain.Shared.ValueObjects;

namespace PetHouse.Domain.Specie.Entities;

public class Breed : Entity<BreedId>
{
    public string Name { get; private set; }
}