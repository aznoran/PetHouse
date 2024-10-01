using Microsoft.EntityFrameworkCore;
using PetHouse.Application.Abstraction;
using PetHouse.Application.Dtos.PetManagment;

namespace PetHouse.Application.PetManagement.Queries.GetVolunteerById;

public class GetVolunteerByIdHandler : IQueryHandler<GetVolunteerByIdQuery, VolunteerDto?>
{
    private readonly IReadDbContext _readDbContext;

    public GetVolunteerByIdHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<VolunteerDto?> Handle(GetVolunteerByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await _readDbContext.Volunteers.SingleOrDefaultAsync(v => v.Id==query.Id, cancellationToken);
    }
}