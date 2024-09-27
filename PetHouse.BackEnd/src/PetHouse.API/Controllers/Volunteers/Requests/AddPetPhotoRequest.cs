using PetHouse.Application.Dtos.Shared;
using PetHouse.Application.Volunteers.Commands.AddPetPhotos;

namespace PetHouse.API.Controllers.Volunteers.Requests;

public record AddPetPhotoRequest(
    IFormFile Photo,
    bool IsMain = false)
{
    public AddPetPhotoCommand ToCommand(Guid volunteerId, Guid petId, UploadFileDto file)
    {
        return new AddPetPhotoCommand(volunteerId, petId, file, IsMain);
    }
}