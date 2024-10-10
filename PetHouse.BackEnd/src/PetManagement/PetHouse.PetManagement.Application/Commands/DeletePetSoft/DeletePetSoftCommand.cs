using PetHouse.Core.Abstraction;

namespace PetHouse.PetManagement.Application.Commands.DeletePetSoft;

public record DeletePetSoftCommand(Guid VolunteerId, Guid PetId) : ICommand;