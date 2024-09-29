using PetHouse.Application.Abstraction;

namespace PetHouse.Application.SpecieManagment.Commands.AddBreed;

public record AddBreedCommand(Guid Id, string Name) : ICommand;
