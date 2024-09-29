using PetHouse.Application.SpecieManagment.Commands.DeleteBreed;

namespace PetHouse.API.Controllers.SpeciesManagment.Requests;

public record DeleteBreedRequest()
{
    public DeleteBreedCommand ToCommand(Guid specieId, Guid breedID)
    {
        return new DeleteBreedCommand(specieId, breedID);
    }
}