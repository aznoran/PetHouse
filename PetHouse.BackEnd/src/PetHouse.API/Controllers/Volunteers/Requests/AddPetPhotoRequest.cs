using PetHouse.Application.Dto;
using PetHouse.Application.Volunteers.AddPetPhoto;

namespace PetHouse.Application.Volunteers.Create;

public record AddPetPhotoRequest(
    IFormFileCollection Photos,
    bool IsMain = false)
{
    public AddPetPhotosCommand ToCommand(Guid volunteerId, Guid petId, IEnumerable<UploadFileDto> files)
    {
        return new AddPetPhotosCommand(volunteerId, petId, files, IsMain);
    }
}