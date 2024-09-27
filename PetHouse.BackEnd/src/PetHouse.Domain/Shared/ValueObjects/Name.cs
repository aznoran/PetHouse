using CSharpFunctionalExtensions;
using PetHouse.Domain.Shared.Constraints;
using PetHouse.Domain.Shared.Other;

namespace PetHouse.Domain.Shared.ValueObjects;

public record Name
{
    private Name(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Name, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > DefaultConstraints.MAX_NAME_LENGTH)
        {
            return Result.Failure<Name, Error>(Errors.General.ValueIsInvalid("name"));
        }

        return new Name(value);
    }
}