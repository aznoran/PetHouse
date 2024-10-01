using PetHouse.Application.Abstraction;

namespace PetHouse.Application.SpecieManagement.Commands.Create;

public record CreateSpecieCommand(string Name) : ICommand;
