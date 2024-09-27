using PetHouse.Application.SpecieManagment.Commands.Create;

namespace PetHouse.API.Controllers.Volunteers.Requests;

public record DeleteBreedRequest()
{
    public DeleteBreedCommand ToCommand(Guid specieId, Guid breedID)
    {
        return new DeleteBreedCommand(specieId, breedID);
    }
}