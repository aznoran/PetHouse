using PetHouse.Application.Abstraction;

namespace PetHouse.Application.Volunteers.Commands.Delete;

public record DeleteVolunteerCommand(Guid Id) : ICommand;