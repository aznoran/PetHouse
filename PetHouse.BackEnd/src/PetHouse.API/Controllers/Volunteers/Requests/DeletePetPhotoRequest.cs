using PetHouse.Application.Volunteers.Commands.AddPetPhotos;

namespace PetHouse.API.Controllers.Volunteers.Requests;

public record DeletePetPhotoRequest(
    string BucketName,
    string FileName)
{
    public DeletePetPhotoCommand ToCommand(Guid volunteerId, Guid petId)
    {
        return new DeletePetPhotoCommand(volunteerId, petId, BucketName, FileName);
    }
}