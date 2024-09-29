using PetHouse.Application.Abstraction;

namespace PetHouse.Application.PetManagement.Commands.Delete;

public record DeleteVolunteerCommand(Guid Id) : ICommand;