using CSharpFunctionalExtensions;
using PetHouse.Domain.Models;
using PetHouse.Domain.Shared;

namespace PetHouse.Domain;

public record PetIdentifier
{
    private PetIdentifier(SpeciesId speciesId, Guid breedId)
    {
        SpeciesId = speciesId;
        BreedId = breedId;
    }
    public SpeciesId SpeciesId { get; }

    public Guid BreedId { get; }

    public static Result<PetIdentifier, Error> Create(Guid speciesId, Guid breedId)
    {
        return new PetIdentifier(SpeciesId.Create(speciesId), breedId);
    }
}