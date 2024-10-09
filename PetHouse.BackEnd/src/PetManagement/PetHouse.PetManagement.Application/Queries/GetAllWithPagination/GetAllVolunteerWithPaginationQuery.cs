using PetHouse.Core.Abstraction;

namespace PetHouse.PetManagement.Application.Queries.GetAllWithPagination;

public record GetAllVolunteerWithPaginationQuery(int Page, int PageSize) : IQuery
{ }