using PetHouse.Application.Abstraction;

namespace PetHouse.Application.Volunteers.Queries.GetAllWithPagination;

public record GetAllSpeciesWithPaginationQuery(int Page, int PageSize) : IQuery
{ }