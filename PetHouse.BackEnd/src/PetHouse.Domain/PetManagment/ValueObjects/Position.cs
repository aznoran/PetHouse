using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using PetHouse.Domain.Shared;

namespace PetHouse.Domain.Models;

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