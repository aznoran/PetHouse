using PetHouse.Core.Abstraction;

namespace PetHouse.SpecieManagement.Application.SpecieManagement.Commands.Delete;

public record DeleteSpecieCommand(Guid Id) : ICommand;
