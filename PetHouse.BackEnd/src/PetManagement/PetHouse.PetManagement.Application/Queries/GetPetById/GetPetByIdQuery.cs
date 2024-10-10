using PetHouse.Core.Abstraction;

namespace PetHouse.PetManagement.Application.Queries.GetPetById;

//На будущее если понадобится фильтрация. На данный момент это просто набросок
public record GetPetByIdQuery() : IQuery
{
    private GetPetByIdQuery(Guid petId) : this()
    {
        PetId = petId;
    }
    internal Guid PetId { get; init; }

    public GetPetByIdQuery GetQueryWithId(Guid petId)
    {
        return new(petId);
    }
}