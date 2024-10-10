using PetHouse.Core.Abstraction;

namespace PetHouse.PetManagement.Application.Commands.DeletePetPhoto;

public record DeletePetPhotoCommand(
    Guid VolunteerId,
    Guid PetId,
    string BucketName,
    string FileName) : ICommand;