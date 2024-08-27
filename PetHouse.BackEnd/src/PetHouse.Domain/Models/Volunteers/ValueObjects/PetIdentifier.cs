using PetHouse.Domain.Models;

namespace PetHouse.Domain;

public record PetIdentifier
{
    public SpeciesId SpeciesId { get; }

    public Guid BreedId { get; }
}