using PetHouse.Application.Abstraction;

namespace PetHouse.Application.Volunteers.Queries.GetAllWithPagination;

public record GetAllVolunteerWithPaginationQuery(int Page, int PageSize) : IQuery
{ }