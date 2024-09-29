using PetHouse.Application.Abstraction;

namespace PetHouse.Application.PetManagement.Commands.DeletePetPhoto;

public record DeletePetPhotoCommand(
    Guid VolunteerId,
    Guid PetId,
    string BucketName,
    string FileName) : ICommand;