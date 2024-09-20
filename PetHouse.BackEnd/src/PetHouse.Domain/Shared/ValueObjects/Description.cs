using CSharpFunctionalExtensions;
using PetHouse.Domain.Constraints;
using PetHouse.Domain.Shared;

namespace PetHouse.Domain.Models.Volunteers.ValueObjects;

public record Description
{
    public Description(string value)
    {
        Value = value;
    }
    
    public string Value { get; }
    
    public static Result<Description, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > DefaultConstraints.MAX_DESCRIPTION_LENGTH)
        {
            return Result.Failure<Description, Error>(Errors.General.ValueIsInvalid("description"));
        }

        return new Description(value);
    }
}