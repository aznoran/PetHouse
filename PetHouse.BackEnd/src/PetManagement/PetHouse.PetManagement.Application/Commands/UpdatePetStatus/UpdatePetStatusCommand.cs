using PetHouse.Core.Abstraction;

namespace PetHouse.PetManagement.Application.Commands.UpdatePetStatus;

public record UpdatePetStatusCommand(
    Guid VolunteerId,
    Guid PetId,
    int PetStatus) : ICommand;
    