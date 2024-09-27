using CSharpFunctionalExtensions;
using PetHouse.Domain.Shared.Id;
using PetHouse.Domain.Shared.ValueObjects;

namespace PetHouse.Domain.PetManagment.ValueObjects;

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