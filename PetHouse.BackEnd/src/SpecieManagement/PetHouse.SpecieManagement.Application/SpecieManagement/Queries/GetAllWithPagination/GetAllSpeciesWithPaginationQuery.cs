using PetHouse.Core.Abstraction;

namespace PetHouse.SpecieManagement.Application.SpecieManagement.Queries.GetAllWithPagination;

public record GetAllSpeciesWithPaginationQuery(int Page, int PageSize) : IQuery
{ }