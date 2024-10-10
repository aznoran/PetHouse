using PetHouse.Core.Abstraction;

namespace PetHouse.PetManagement.Application.Commands.Delete;

public record DeleteVolunteerCommand(Guid Id) : ICommand;