using PetHouse.Core.Abstraction;

namespace PetHouse.SpecieManagement.Application.SpecieManagement.Queries.GetBreedById;

public record GetBreedByIdQuery() : IQuery
{
    private GetBreedByIdQuery(Guid id, int page, int pageSize) : this()
    {
        Id = id;
        Page = page;
        PageSize = pageSize;
    }
    
    public int Page { get; init; }
    public int PageSize { get; init; }
    internal Guid Id { get; init; }

    public GetBreedByIdQuery GetQueryWithId(Guid id)
    {
        return new(id, Page, PageSize);
    }
}