using PetHouse.Application.Abstraction;

namespace PetHouse.Application.SpecieManagment.Commands.Create;

public record DeleteBreedCommand(Guid SpecieId, Guid BreedId) : ICommand;
