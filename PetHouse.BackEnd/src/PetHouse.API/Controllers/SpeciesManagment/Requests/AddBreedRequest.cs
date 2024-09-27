using PetHouse.Infrastructure.Repositories.Commands.AddBreed;

namespace PetHouse.API.Controllers.Volunteers.Requests;

public record AddBreedRequest(
    string Name)
{
    public AddBreedCommand ToCommand(Guid id)
    {
        return new AddBreedCommand(id, Name);
    }
}