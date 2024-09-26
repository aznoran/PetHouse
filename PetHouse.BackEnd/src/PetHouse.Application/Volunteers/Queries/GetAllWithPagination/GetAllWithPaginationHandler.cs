using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetHouse.Application.Abstraction;
using PetHouse.Application.Dtos.PetManagment;
using PetHouse.Application.Extensions;
using PetHouse.Domain.Shared.Other;

namespace PetHouse.Application.Volunteers.Queries.GetAllWithPagination;

public class GetAllWithPaginationHandler : IQueryHandler<GetAllWithPaginationQuery, PagedList<VolunteerDto>>
{
    private readonly IReadDbContext _readDbContext;

    public GetAllWithPaginationHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<PagedList<VolunteerDto>> Handle(GetAllWithPaginationQuery query,
        CancellationToken cancellationToken = default)
    {
        return await _readDbContext.Volunteers.ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
}


