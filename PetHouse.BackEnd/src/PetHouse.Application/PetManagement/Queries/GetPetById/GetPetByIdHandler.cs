using Microsoft.EntityFrameworkCore;
using PetHouse.Application.Abstraction;
using PetHouse.Application.Dtos.PetManagment;

namespace PetHouse.Application.PetManagement.Queries.GetPetById;

public class GetPetByIdHandler : IQueryHandler<GetPetByIdQuery, PetDto>
{
    private readonly IReadDbContext _readDbContext;

    public GetPetByIdHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<PetDto> Handle(GetPetByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return (await _readDbContext.Pets.SingleOrDefaultAsync(p =>
            p.Id == query.PetId && p.VolunteerId == query.VolunteerId
            , cancellationToken))!;
    }
}