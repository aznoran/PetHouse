using PetHouse.Application.Abstraction;
using PetHouse.Application.Dtos.Shared;

namespace PetHouse.Application.Volunteers.Commands.AddPetPhotos;

public record AddPetPhotoCommand(
    Guid VolunteerId,
    Guid PetId,
    UploadFileDto File,
    bool IsMain = false) : ICommand;