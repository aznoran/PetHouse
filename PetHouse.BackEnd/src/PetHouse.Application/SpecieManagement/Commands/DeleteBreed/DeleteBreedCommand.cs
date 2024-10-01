using PetHouse.Application.Abstraction;

namespace PetHouse.Application.SpecieManagement.Commands.DeleteBreed;

public record DeleteBreedCommand(Guid SpecieId, Guid BreedId) : ICommand;
