using PetHouse.SharedKernel.Id;
using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.SpecieManagement.Domain.Entities;

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