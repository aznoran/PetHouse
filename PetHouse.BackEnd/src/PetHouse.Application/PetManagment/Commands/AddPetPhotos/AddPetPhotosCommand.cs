using PetHouse.Application.Abstraction;
using PetHouse.Application.Dtos.Shared;

namespace PetHouse.Application.PetManagment.Commands.AddPetPhotos;

public record AddPetPhotosCommand(
    Guid VolunteerId,
    Guid PetId,
    IEnumerable<UploadFileDto> Files,
    bool IsMain = false) : ICommand;