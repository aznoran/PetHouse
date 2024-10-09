using CSharpFunctionalExtensions;
using PetHouse.SharedKernel.Id;
using PetHouse.SharedKernel.Other;
using PetHouse.SharedKernel.ValueObjects;
using PetHouse.SpecieManagement.Domain.Entities;

namespace PetHouse.SpecieManagement.Domain.Aggregate;

public sealed class Specie : SharedKernel.ValueObjects.Entity<SpeciesId>
{
    //EF CORE
    // ReSharper disable once UnusedMember.Local
    private Specie(SpeciesId id) : base(id){}
    private Specie(SpeciesId id,Name name) : base(id)
    {
        Name = name;
    }
    
    public Name Name { get; private set; }

    private List<Breed> _breeds = [];
    public IReadOnlyList<Breed> Breeds => _breeds;

    public static Result<Specie, Error> Create(SpeciesId id, Name name)
    {
        return new Specie(id,name);
    }

    public Result<Guid, Error> AddBreed(Breed breed)
    {
        if (Breeds.FirstOrDefault(b => b.Name.Value == breed.Name.Value) is not null)
        {
            return Errors.Specie.BreedAlreadyExists();
        }
        _breeds.Add(breed);
        return breed.Id.Value;
    }
    
    public Result<Guid, Error> RemoveBreed(BreedId breedId)
    {
        var breedRes = Breeds.FirstOrDefault(b => b.Id == breedId);
        if (breedRes is null)
        {
            return Errors.General.NotFound();
        }

        _breeds.Remove(breedRes);
        
        return breedId.Value;
    }
}