namespace PetHouse.Core.Abstraction;

public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery
{
    Task<TResult> Handle(TQuery command, CancellationToken cancellationToken);
}