using PetHouse.Application.Abstraction;

namespace PetHouse.Application.PetManagement.Commands.DeletePetSoft;

public record DeletePetSoftCommand(Guid VolunteerId, Guid PetId) : ICommand;