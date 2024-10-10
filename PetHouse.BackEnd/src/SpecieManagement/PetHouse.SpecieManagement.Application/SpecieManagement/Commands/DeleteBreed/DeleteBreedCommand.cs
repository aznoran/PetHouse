using PetHouse.Core.Abstraction;

namespace PetHouse.SpecieManagement.Application.SpecieManagement.Commands.DeleteBreed;

public record DeleteBreedCommand(Guid SpecieId, Guid BreedId) : ICommand;
