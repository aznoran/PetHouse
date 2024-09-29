using PetHouse.Application.Abstraction;

namespace PetHouse.Application.PetManagement.Queries.GetAllWithPagination;

public record GetAllVolunteerWithPaginationQuery(int Page, int PageSize) : IQuery
{ }