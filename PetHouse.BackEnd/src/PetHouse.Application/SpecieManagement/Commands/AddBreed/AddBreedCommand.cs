using PetHouse.Application.Abstraction;

namespace PetHouse.Application.SpecieManagement.Commands.AddBreed;

public record AddBreedCommand(Guid Id, string Name) : ICommand;
