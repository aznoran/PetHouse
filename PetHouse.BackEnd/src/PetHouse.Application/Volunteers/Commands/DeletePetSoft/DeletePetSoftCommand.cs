using PetHouse.Application.Abstraction;

namespace PetHouse.Application.Volunteers.Commands.Delete;

public record DeletePetSoftCommand(Guid VolunteerId, Guid PetId) : ICommand;