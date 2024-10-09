using CSharpFunctionalExtensions;
using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.Core.Abstraction;

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    Task<UnitResult<ErrorList>> Handle(TCommand commandInput, CancellationToken cancellationToken);
}

public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand
{
    Task<Result<TResult, ErrorList>> Handle(TCommand command, CancellationToken cancellationToken);
}