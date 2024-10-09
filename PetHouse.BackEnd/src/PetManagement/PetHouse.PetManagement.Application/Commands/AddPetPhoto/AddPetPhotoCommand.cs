using PetHouse.Core.Abstraction;
using PetHouse.Core.Dtos.Shared;

namespace PetHouse.PetManagement.Application.Commands.AddPetPhoto;

public record AddPetPhotoCommand(
    Guid VolunteerId,
    Guid PetId,
    UploadFileDto File,
    bool IsMain = false) : ICommand;