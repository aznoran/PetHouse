using PetHouse.Application.Abstraction;
using PetHouse.Application.Dtos.Shared;

namespace PetHouse.Application.PetManagement.Commands.AddPetPhoto;

public record AddPetPhotoCommand(
    Guid VolunteerId,
    Guid PetId,
    UploadFileDto File,
    bool IsMain = false) : ICommand;