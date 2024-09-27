using PetHouse.Application.SpecieManagment.Commands.Delete;

namespace PetHouse.API.Controllers.SpeciesManagment.Requests;

public record DeleteSpecieRequest()
{
    public DeleteSpecieCommand ToCommand(Guid id)
    {
        return new DeleteSpecieCommand(id);
    }
}