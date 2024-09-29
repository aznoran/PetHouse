using PetHouse.Application.Abstraction;

namespace PetHouse.Application.SpecieManagment.Commands.Create;

public record CreateSpecieCommand(string Name) : ICommand;
