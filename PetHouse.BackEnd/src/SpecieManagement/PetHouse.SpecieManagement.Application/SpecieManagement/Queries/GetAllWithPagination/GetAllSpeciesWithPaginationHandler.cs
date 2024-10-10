using PetHouse.Core.Abstraction;
using PetHouse.Core.Dtos.SpeciesManagment;
using PetHouse.Core.Extensions;
using PetHouse.Core.Models;

namespace PetHouse.SpecieManagement.Application.SpecieManagement.Queries.GetAllWithPagination;

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