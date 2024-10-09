using PetHouse.Core.Abstraction;

namespace PetHouse.SpecieManagement.Application.SpecieManagement.Commands.Create;

public record CreateSpecieCommand(string Name) : ICommand;
