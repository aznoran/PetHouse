using PetHouse.Domain.Shared.Id;
using PetHouse.Domain.Shared.ValueObjects;

namespace PetHouse.Domain.Specie.Entities;

public class Breed : Entity<BreedId>
{
    //EF CORE NAVIGATION PROPERTY
    public Aggregate.Specie Specie { get; init; }
    //EF CORE
    // ReSharper disable once UnusedMember.Local
    private Breed(BreedId id) : base(id){}
    private Breed(BreedId id, Name name) : base(id)
    {
        Name = name;
    }
    
    public Name Name { get; private set; }

    public static Breed Create(BreedId id, Name name)
    {
        return new(id, name);
    }
}