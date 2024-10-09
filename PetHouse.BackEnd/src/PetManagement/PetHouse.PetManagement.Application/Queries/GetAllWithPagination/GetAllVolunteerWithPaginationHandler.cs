using PetHouse.Core.Abstraction;
using PetHouse.Core.Dtos.PetManagment;
using PetHouse.Core.Extensions;
using PetHouse.Core.Models;

namespace PetHouse.PetManagement.Application.Queries.GetAllWithPagination;

public class GetAllVolunteerWithPaginationHandler : IQueryHandler<GetAllVolunteerWithPaginationQuery, PagedList<VolunteerDto>>
{
    private readonly IReadDbContext _readDbContext;

    public GetAllVolunteerWithPaginationHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<PagedList<VolunteerDto>> Handle(GetAllVolunteerWithPaginationQuery query,
        CancellationToken cancellationToken = default)
    {
        return await _readDbContext.Volunteers.ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
}


