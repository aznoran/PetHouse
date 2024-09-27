using PetHouse.Application.Abstraction;

namespace PetHouse.Application.SpecieManagment.Commands.Create;

public record DeleteSpecieCommand(Guid Id) : ICommand;
