using PetHouse.Application.Abstraction;

namespace PetHouse.Application.PetManagement.Commands.ChangePetMainPhoto;

public record ChangePetMainPhotoCommand(
    Guid VolunteerId,
    Guid PetId,
    string FileName) : ICommand;