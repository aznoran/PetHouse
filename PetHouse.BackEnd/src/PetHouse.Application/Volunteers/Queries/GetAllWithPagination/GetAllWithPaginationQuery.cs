using PetHouse.Application.Abstraction;

namespace PetHouse.Application.Volunteers.Queries.GetAllWithPagination;

public record GetAllWithPaginationQuery(int Page, int PageSize) : IQuery
{ }