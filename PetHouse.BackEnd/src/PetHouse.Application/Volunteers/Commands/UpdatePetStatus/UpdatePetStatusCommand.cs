using PetHouse.Application.Abstraction;
using PetHouse.Application.Dtos.PetManagment;
using PetHouse.Domain.PetManagment.Enums;

namespace PetHouse.Application.Volunteers.Commands.AddPet;

public record UpdatePetStatusCommand(
    Guid VolunteerId,
    Guid PetId,
    int PetStatus) : ICommand;
    