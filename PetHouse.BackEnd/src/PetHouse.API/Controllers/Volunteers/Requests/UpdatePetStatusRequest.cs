using PetHouse.Application.PetManagement.Commands.UpdatePetStatus;

namespace PetHouse.API.Controllers.Volunteers.Requests;

public record UpdatePetStatusRequest(
    int PetStatus)
{
    public UpdatePetStatusCommand ToCommand(Guid volunteerId, Guid petId)
    {
        return new UpdatePetStatusCommand(volunteerId, petId, PetStatus);
    }
}