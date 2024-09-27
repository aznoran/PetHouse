using PetHouse.Application.Abstraction;
using PetHouse.Application.Dtos.Shared;

namespace PetHouse.Application.Volunteers.Commands.AddPetPhotos;

public record DeletePetPhotoCommand(
    Guid VolunteerId,
    Guid PetId,
    string BucketName,
    string FileName) : ICommand;