using Microsoft.EntityFrameworkCore;
using PetHouse.Application.Abstraction;
using PetHouse.Application.Dtos.PetManagment;
using PetHouse.Application.Extensions;
using PetHouse.Application.Volunteers.Queries.GetAllWithPagination;
using PetHouse.Domain.Shared.Other;

namespace PetHouse.Application.Volunteers.Queries.GetVolunteerById;

public class GetVolunteerByIdHandler : IQueryHandler<GetVolunteerByIdQuery, VolunteerDto>
{
    private readonly IReadDbContext _readDbContext;

    public GetVolunteerByIdHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<VolunteerDto> Handle(GetVolunteerByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return (await _readDbContext.Volunteers.SingleOrDefaultAsync(v => v.Id==query.Id, cancellationToken))!;
    }
}