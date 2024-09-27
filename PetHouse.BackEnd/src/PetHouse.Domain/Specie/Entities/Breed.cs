using PetHouse.Domain.PetManagment.ValueObjects;
using PetHouse.Domain.Shared.Id;
using PetHouse.Domain.Shared.Other;
using PetHouse.Domain.Shared.ValueObjects;

namespace PetHouse.Domain.Specie.Entities;

public class Breed : Entity<BreedId>
{
    //EF CORE
    private Breed(){}
    private Breed(BreedId id, Name name) : base(id)
    {
        Name = name;
    }
    
    public Name Name { get; private set; }
    
    public Specie Specie { get; init; }

    public static Breed Create(BreedId id, Name name)
    {
        return new(id, name);
    }
}