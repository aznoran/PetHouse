namespace PetHouse.Application.Dtos;

public record PetIdentifierDto(
    Guid SpeciesId,
    Guid BreedId);