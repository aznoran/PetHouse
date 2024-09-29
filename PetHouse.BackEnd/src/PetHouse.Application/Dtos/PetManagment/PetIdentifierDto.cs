namespace PetHouse.Application.Dtos.PetManagment;

public record PetIdentifierDto(
    Guid SpeciesId,
    Guid BreedId);