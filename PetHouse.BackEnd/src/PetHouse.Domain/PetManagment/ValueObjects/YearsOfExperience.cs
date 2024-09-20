using CSharpFunctionalExtensions;
using PetHouse.Domain.Constraints;
using PetHouse.Domain.Shared;

namespace PetHouse.Domain.Models.Volunteers.ValueObjects;

public record YearsOfExperience
{
    private YearsOfExperience(int value)
    {
        Value = value;
    }
    
    public int Value { get; }
    
    public static Result<YearsOfExperience, Error> Create(int value)
    {
        if (int.IsNegative(value) || value > 50)
        {
            return Result.Failure<YearsOfExperience, Error>(Errors.General.ValueIsInvalid("yearsOfExperience"));
        }

        return new YearsOfExperience(value);
    }
}