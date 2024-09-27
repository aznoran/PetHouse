using PetHouse.Application.Abstraction;

namespace PetHouse.Application.PetManagment.Queries.GetAllWithPagination;

public record GetAllVolunteerWithPaginationQuery(int Page, int PageSize) : IQuery
{ }