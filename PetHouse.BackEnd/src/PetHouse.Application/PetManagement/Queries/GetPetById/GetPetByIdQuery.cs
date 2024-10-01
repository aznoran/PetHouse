using PetHouse.Application.Abstraction;

namespace PetHouse.Application.PetManagement.Queries.GetPetById;

//На будущее если понадобится фильтрация. На данный момент это просто набросок
public record GetPetByIdQuery() : IQuery
{
    private GetPetByIdQuery(Guid volunteerId, Guid petId) : this()
    {
        VolunteerId = volunteerId;
        PetId = petId;
    }
    internal Guid PetId { get; init; }

    internal Guid VolunteerId { get; init; }

    public GetPetByIdQuery GetQueryWithId(Guid volunteerId, Guid petId)
    {
        return new(volunteerId, petId);
    }
}