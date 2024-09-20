using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using PetHouse.Domain.Shared;

namespace PetHouse.Domain.Models.Volunteers.ValueObjects;

public record PhoneNumber
{
    private PhoneNumber(string value)
    {
        Value = value;
    }
    
    public string Value { get; }
    
    public static Result<PhoneNumber, Error> Create(string value)
    {
        if (!Regex.IsMatch(value, @"(^8|7|\+7)((\d{10})|(\s\(\d{3}\)\s\d{3}\s\d{2}\s\d{2}))"))
        {
            return Result.Failure<PhoneNumber, Error>(Errors.Volunteer.WrongPhoneNumber(value));
        }

        return new PhoneNumber(value);
    }
}