namespace PetHouse.Core.Dtos.PetManagment;

public record PetIdentifierDto(
    Guid SpeciesId,
    Guid BreedId);