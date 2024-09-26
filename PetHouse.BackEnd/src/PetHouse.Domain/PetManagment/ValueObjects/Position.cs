using CSharpFunctionalExtensions;
using PetHouse.Domain.Shared.ValueObjects;

namespace PetHouse.Domain.PetManagment.ValueObjects;

public record Position
{
    private Position(int value)
    {
        Value = value;
    }
    
    public int Value { get; }
    
    public static Result<Position, Error> Create(int value)
    {
        if (int.IsNegative(value))
        {
            value = 1;
        }

        return new Position(value);
    }
}