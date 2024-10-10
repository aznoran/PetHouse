using PetHouse.Core.Abstraction;
using PetHouse.Core.Dtos.Shared;

namespace PetHouse.PetManagement.Application.Commands.AddPetPhotos;

public record AddPetPhotosCommand(
    Guid VolunteerId,
    Guid PetId,
    IEnumerable<UploadFileDto> Files,
    bool IsMain = false) : ICommand;