using CSharpFunctionalExtensions;
using PetHouse.SharedKernel.Other;
using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.PetManagement.Domain.ValueObjects;

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