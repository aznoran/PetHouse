using PetHouse.Core.Abstraction;
using PetHouse.Core.Dtos.SpeciesManagment;
using PetHouse.Core.Extensions;
using PetHouse.Core.Models;

namespace PetHouse.SpecieManagement.Application.SpecieManagement.Queries.GetBreedById;

public class GetBreedByIdHandler: IQueryHandler<GetBreedByIdQuery, PagedList<BreedDto>>
{
    private readonly IReadDbContext _readDbContext;

    public GetBreedByIdHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<PagedList<BreedDto>> Handle(GetBreedByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await _readDbContext.Breeds.ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
}