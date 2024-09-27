using PetHouse.Application.Dtos.Shared;
using PetHouse.Application.Volunteers.Commands.AddPetPhotos;

namespace PetHouse.API.Controllers.Volunteers.Requests;

public record AddPetPhotosRequest(
    IFormFileCollection Photos,
    bool IsMain = false)
{
    public AddPetPhotosCommand ToCommand(Guid volunteerId, Guid petId, IEnumerable<UploadFileDto> files)
    {
        return new AddPetPhotosCommand(volunteerId, petId, files, IsMain);
    }
}