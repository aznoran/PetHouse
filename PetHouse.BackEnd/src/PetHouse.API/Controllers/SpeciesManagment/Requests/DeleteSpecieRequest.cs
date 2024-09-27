using PetHouse.Application.SpecieManagment.Commands.Create;

namespace PetHouse.API.Controllers.Volunteers.Requests;

public record DeleteSpecieRequest()
{
    public DeleteSpecieCommand ToCommand(Guid id)
    {
        return new DeleteSpecieCommand(id);
    }
}