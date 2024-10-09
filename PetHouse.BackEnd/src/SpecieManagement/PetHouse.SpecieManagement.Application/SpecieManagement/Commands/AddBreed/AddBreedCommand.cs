using PetHouse.Core.Abstraction;

namespace PetHouse.SpecieManagement.Application.SpecieManagement.Commands.AddBreed;

public record AddBreedCommand(Guid Id, string Name) : ICommand;
