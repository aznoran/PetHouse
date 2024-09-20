using CSharpFunctionalExtensions;
using PetHouse.Domain.Constraints;
using PetHouse.Domain.Shared;

namespace PetHouse.Domain.Models.Volunteers.ValueObjects;

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