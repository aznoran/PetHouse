using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using PetHouse.Domain.Constraints;
using PetHouse.Domain.Shared;

namespace PetHouse.Domain.Models;

public record VolunteerProfile
{
    private VolunteerProfile(
        string fullName,
        string description, 
        int yearsOfExperience, 
        string phoneNumber)
    {
        FullName = fullName;
        Description = description;
        YearsOfExperience = yearsOfExperience;
        PhoneNumber = phoneNumber;
    }

    public string FullName { get; }

    public string Description { get; }

    public int YearsOfExperience { get; }

    public string PhoneNumber { get; }

    public static Result<VolunteerProfile, Error> Create(
        string fullName, 
        string description,
        int yearsOfExperience, 
        string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(fullName) || fullName.Length > DefaultConstraints.MAX_LINK_LENGTH)
        {
            return Result.Failure<VolunteerProfile, Error>(Errors.General.ValueIsInvalid(nameof(fullName)));
        }

        if (string.IsNullOrWhiteSpace(description) || description.Length > DefaultConstraints.MAX_DESCRIPTION_LENGTH)
        {
            return Result.Failure<VolunteerProfile, Error>(Errors.General.ValueIsInvalid(nameof(description)));
        }

        if (int.IsNegative(yearsOfExperience) || yearsOfExperience > 50)
        {
            return Result.Failure<VolunteerProfile, Error>(Errors.General.ValueIsInvalid(nameof(yearsOfExperience)));
        }

        if (!Regex.IsMatch(phoneNumber, "(^8|7|\\+7)((\\d{10})|(\\s\\(\\d{3}\\)\\s\\d{3}\\s\\d{2}\\s\\d{2}))"))
        {
            return Result.Failure<VolunteerProfile, Error>(Errors.General.WrongPhoneNumber(phoneNumber));
        }

        return new VolunteerProfile(
            fullName, 
            description, 
            yearsOfExperience,
            phoneNumber);
    }
}