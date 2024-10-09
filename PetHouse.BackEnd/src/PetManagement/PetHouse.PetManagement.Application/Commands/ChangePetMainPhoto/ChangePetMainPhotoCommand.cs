using PetHouse.Core.Abstraction;

namespace PetHouse.PetManagement.Application.Commands.ChangePetMainPhoto;

public record ChangePetMainPhotoCommand(
    Guid VolunteerId,
    Guid PetId,
    string FileName) : ICommand;