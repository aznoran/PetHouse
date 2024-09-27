using PetHouse.Application.Abstraction;

namespace PetHouse.Application.SpecieManagment.Queries.GetAllWithPagination;

public record GetAllSpeciesWithPaginationQuery(int Page, int PageSize) : IQuery
{ }