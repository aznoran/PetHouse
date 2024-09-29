using PetHouse.Application.Abstraction;

namespace PetHouse.Application.PetManagement.Commands.DeletePet;

public record DeletePetCommand(Guid VolunteerId, Guid PetId) : ICommand;