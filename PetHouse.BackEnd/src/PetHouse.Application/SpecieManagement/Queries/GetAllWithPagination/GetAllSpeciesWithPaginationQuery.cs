using PetHouse.Application.Abstraction;

namespace PetHouse.Application.SpecieManagement.Queries.GetAllWithPagination;

public record GetAllSpeciesWithPaginationQuery(int Page, int PageSize) : IQuery
{ }