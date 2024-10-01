using PetHouse.Application.PetManagement.Commands.ChangePetMainPhoto;

namespace PetHouse.API.Controllers.Volunteers.Requests;

public record ChangePetMainPhotoRequest(
    string FileName)
{
    public ChangePetMainPhotoCommand ToCommand(Guid volunteerId, Guid petId)
    {
        return new ChangePetMainPhotoCommand(
            volunteerId,
            petId,
            FileName);
    }
}