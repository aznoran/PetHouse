using System.Text.RegularExpressions;
using PetHouse.Domain.Constraints;

namespace PetHouse.Domain.Models;

public record VolunteerProfile
{
    private VolunteerProfile(string fullName, string description, int yearsOfExperience, int countOfPetsFoundHome,
        int countOfPetsLookingForHome, int countOfPetsOnTreatment, string phoneNumber)
    {
        FullName = fullName;
        Description = description;
        YearsOfExperience = yearsOfExperience;
        CountOfPetsFoundHome = countOfPetsFoundHome;
        CountOfPetsLookingForHome = countOfPetsLookingForHome;
        CountOfPetsOnTreatment = countOfPetsOnTreatment;
        PhoneNumber = phoneNumber;
    }

    public string FullName { get; }

    public string Description { get; }

    public int YearsOfExperience { get; }

    public int CountOfPetsFoundHome { get; }

    public int CountOfPetsLookingForHome { get; }

    public int CountOfPetsOnTreatment { get; }

    public string PhoneNumber { get; }

    public static VolunteerProfile Create(string fullName, string description, int yearsOfExperience,
        int countOfPetsFoundHome, int countOfPetsLookingForHome, int countOfPetsOnTreatment, string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(fullName) || fullName.Length > DefaultConstraints.MAX_REFERENCE_LENGTH)
        {
            throw new Exception("VolunteerProfile creation error : fullName");
        }

        if (string.IsNullOrWhiteSpace(description) || description.Length > DefaultConstraints.MAX_DESCRIPTION_LENGTH)
        {
            throw new Exception("VolunteerProfile creation error : description");
        }

        if (int.IsNegative(yearsOfExperience) || yearsOfExperience > 50)
        {
            throw new Exception("VolunteerProfile creation error : yearsOfExperience");
        }

        if (int.IsNegative(countOfPetsFoundHome))
        {
            throw new Exception("VolunteerProfile creation error : countOfPetsFoundHome");
        }

        if (int.IsNegative(countOfPetsLookingForHome))
        {
            throw new Exception("VolunteerProfile creation error : countOfPetsLookingForHome");
        }

        if (int.IsNegative(countOfPetsOnTreatment))
        {
            throw new Exception("VolunteerProfile creation error : countOfPetsOnTreatment");
        }

        if (!Regex.IsMatch(phoneNumber, "(^8|7|\\+7)((\\d{10})|(\\s\\(\\d{3}\\)\\s\\d{3}\\s\\d{2}\\s\\d{2}))"))
        {
            throw new Exception("VolunteerProfile creation error : phoneNumber");
        }

        return new VolunteerProfile(fullName, description, yearsOfExperience,
            countOfPetsFoundHome, countOfPetsLookingForHome, countOfPetsOnTreatment, phoneNumber);
    }
}