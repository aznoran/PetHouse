using PetHouse.Application.Abstraction;

namespace PetHouse.Application.SpecieManagement.Commands.Delete;

public record DeleteSpecieCommand(Guid Id) : ICommand;
