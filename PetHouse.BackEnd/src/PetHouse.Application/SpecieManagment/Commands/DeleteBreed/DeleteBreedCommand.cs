using PetHouse.Application.Abstraction;

namespace PetHouse.Application.SpecieManagment.Commands.DeleteBreed;

public record DeleteBreedCommand(Guid SpecieId, Guid BreedId) : ICommand;
