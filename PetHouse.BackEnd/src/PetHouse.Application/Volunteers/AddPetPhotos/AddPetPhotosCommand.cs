using Microsoft.AspNetCore.Http;
using PetHouse.Application.Abstraction;
using PetHouse.Application.Dto;

namespace PetHouse.Application.Volunteers.AddPetPhoto;

public record AddPetPhotosCommand(
    Guid VolunteerId,
    Guid PetId,
    IEnumerable<UploadFileDto> Files,
    bool IsMain = false) : ICommand;