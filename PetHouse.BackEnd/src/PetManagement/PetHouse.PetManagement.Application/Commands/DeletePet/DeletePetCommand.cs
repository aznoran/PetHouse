using PetHouse.Core.Abstraction;

namespace PetHouse.PetManagement.Application.Commands.DeletePet;

public record DeletePetCommand(Guid VolunteerId, Guid PetId) : ICommand;