using PetHouse.Application.Abstraction;

namespace PetHouse.Application.Volunteers.Commands.Delete;

public record DeletePetCommand(Guid VolunteerId, Guid PetId) : ICommand;