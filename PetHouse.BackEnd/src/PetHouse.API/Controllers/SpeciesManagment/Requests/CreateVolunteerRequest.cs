using PetHouse.Application.SpecieManagment.Commands.Create;

namespace PetHouse.API.Controllers.Volunteers.Requests;

public record CreateSpecieRequest(
    string Name)
{
    public CreateSpecieCommand ToCommand()
    {
        return new CreateSpecieCommand(Name);
    }
}