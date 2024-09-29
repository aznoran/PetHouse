using PetHouse.Application.Abstraction;
using PetHouse.Application.Dtos.SpeciesManagment;
using PetHouse.Application.Extensions;
using PetHouse.Domain.Shared.Other;

namespace PetHouse.Application.SpecieManagment.Queries.GetBreedById;

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