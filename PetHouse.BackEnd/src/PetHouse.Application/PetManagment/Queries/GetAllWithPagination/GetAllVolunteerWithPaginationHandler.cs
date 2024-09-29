using PetHouse.Application.Abstraction;
using PetHouse.Application.Dtos.PetManagment;
using PetHouse.Application.Extensions;
using PetHouse.Domain.Shared.Other;

namespace PetHouse.Application.PetManagment.Queries.GetAllWithPagination;

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


