using PetHouse.Application.Abstraction;

namespace PetHouse.Application.PetManagement.Queries.GetVolunteerById;

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