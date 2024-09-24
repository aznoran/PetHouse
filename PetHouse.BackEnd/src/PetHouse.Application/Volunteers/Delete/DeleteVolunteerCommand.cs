using PetHouse.Application.Abstraction;

namespace PetHouse.Application.Volunteers.Delete;

public record DeleteVolunteerCommand(Guid Id) : ICommand;