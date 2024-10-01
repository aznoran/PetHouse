using CSharpFunctionalExtensions;
using PetHouse.Domain.Shared.Other;
using PetHouse.Domain.Shared.ValueObjects;

namespace PetHouse.Domain.PetManagement.ValueObjects;

public record Address
{
    private const int MAX_CITY_LENGTH = 15;
    private const int MAX_STREET_LENGTH = 45;
    private const int MAX_COUNTRY_LENGTH = 20;
    private Address(string? city, string? street, string? country)
    {
        City = city;
        Street = street;
        Country = country;
    }

    public string? City { get; }
    public string? Street { get; }
    
    public string? Country { get; }
    
    public static Result<Address, Error> Create(string? city, string? street, string? country)
    {
        if (string.IsNullOrWhiteSpace(city) == false && city.Length > MAX_CITY_LENGTH)
        {
            return Errors.General.ValueIsInvalid("city");
        }
        if (string.IsNullOrWhiteSpace(street) == false && street.Length > MAX_STREET_LENGTH)
        {
            return Errors.General.ValueIsInvalid("city");
        }
        if (string.IsNullOrWhiteSpace(country) == false && country.Length > MAX_COUNTRY_LENGTH)
        {
            return Errors.General.ValueIsInvalid("country");
        }

        return new Address(city, street, country);
    }
}