using PetHouse.Application.Abstraction;
using PetHouse.Application.Dtos.SpeciesManagment;
using PetHouse.Application.Extensions;
using PetHouse.Domain.Shared.Other;

namespace PetHouse.Application.SpecieManagment.Queries.GetAllWithPagination;

public class GetAllSpeciesWithPaginationHandler : IQueryHandler<GetAllSpeciesWithPaginationQuery, PagedList<SpecieDto>>
{
    private readonly IReadDbContext _readDbContext;

    public GetAllSpeciesWithPaginationHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<PagedList<SpecieDto>> Handle(GetAllSpeciesWithPaginationQuery query,
        CancellationToken cancellationToken = default)
    {
        return await _readDbContext.Species.ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
}