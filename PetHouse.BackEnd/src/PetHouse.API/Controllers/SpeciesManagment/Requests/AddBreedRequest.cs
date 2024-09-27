using PetHouse.Application.SpecieManagment.Commands.AddBreed;

namespace PetHouse.API.Controllers.SpeciesManagment.Requests;

public record AddBreedRequest(
    string Name)
{
    public AddBreedCommand ToCommand(Guid id)
    {
        return new AddBreedCommand(id, Name);
    }
}