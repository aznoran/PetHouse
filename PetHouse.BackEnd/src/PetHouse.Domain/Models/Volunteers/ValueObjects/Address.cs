using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using PetHouse.Domain.Shared;

namespace PetHouse.Domain.Models.Volunteers.ValueObjects;

public class Address
{
    private const int MAX_CITY_LENGTH = 15;
    private const int MAX_STREET_LENGTH = 45;
    private Address(string? city, string? street)
    {
        City = city;
        Street = street;
    }

    public string? City { get; }
    public string? Street { get; }
    
    public static Result<Address, Error> Create(string? city, string? street)
    {
        if (string.IsNullOrWhiteSpace(city) == false && city.Length > MAX_CITY_LENGTH)
        {
            return Errors.General.ValueIsInvalid("city");
        }
        if (string.IsNullOrWhiteSpace(street) == false && street.Length > MAX_STREET_LENGTH)
        {
            return Errors.General.ValueIsInvalid("city");
        }

        return new Address(city, street);
    }
}