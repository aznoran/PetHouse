using PetHouse.Core.Abstraction;

namespace PetHouse.Accounts.Application.Queries.GetAccountById;

public record GetAccountByIdQuery() : IQuery
{
    private GetAccountByIdQuery(Guid userId) : this()
    {
        UserId = userId;
    }
    internal Guid UserId { get; init; }

    public GetAccountByIdQuery GetQueryWithId(Guid userId)
    {
        return new(userId);
    }
}