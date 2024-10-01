using PetHouse.Application.SpecieManagement.Commands.Create;

namespace PetHouse.API.Controllers.SpeciesManagment.Requests;

public record CreateSpecieRequest(
    string Name)
{
    public CreateSpecieCommand ToCommand()
    {
        return new CreateSpecieCommand(Name);
    }
}