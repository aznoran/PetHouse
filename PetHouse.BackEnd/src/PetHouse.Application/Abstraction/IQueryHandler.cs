﻿using CSharpFunctionalExtensions;
using PetHouse.Domain.Shared.ValueObjects;

namespace PetHouse.Application.Abstraction;

public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery
{
    Task<TResult> Handle(TQuery command, CancellationToken cancellationToken);
}