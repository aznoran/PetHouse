using PetHouse.Core.Abstraction;

namespace PetHouse.PetManagement.Application.Queries.GetVolunteerById;

//На будущее если понадобится фильтрация. На данный момент это просто набросок
public record GetVolunteerByIdQuery() : IQuery
{
    private GetVolunteerByIdQuery(Guid id) : this()
    {
        Id = id;
    }

    internal Guid Id { get; init; }

    public GetVolunteerByIdQuery GetQueryWithId(Guid id)
    {
        return new(id);
    }
}