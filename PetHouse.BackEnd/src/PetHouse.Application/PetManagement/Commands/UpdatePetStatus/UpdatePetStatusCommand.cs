using PetHouse.Application.Abstraction;

namespace PetHouse.Application.PetManagement.Commands.UpdatePetStatus;

public record UpdatePetStatusCommand(
    Guid VolunteerId,
    Guid PetId,
    int PetStatus) : ICommand;
    