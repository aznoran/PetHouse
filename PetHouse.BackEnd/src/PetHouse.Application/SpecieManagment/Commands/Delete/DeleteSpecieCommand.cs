using PetHouse.Application.Abstraction;

namespace PetHouse.Application.SpecieManagment.Commands.Delete;

public record DeleteSpecieCommand(Guid Id) : ICommand;
