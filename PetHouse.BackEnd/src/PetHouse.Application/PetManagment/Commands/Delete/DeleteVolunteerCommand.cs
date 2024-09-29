using PetHouse.Application.Abstraction;

namespace PetHouse.Application.PetManagment.Commands.Delete;

public record DeleteVolunteerCommand(Guid Id) : ICommand;