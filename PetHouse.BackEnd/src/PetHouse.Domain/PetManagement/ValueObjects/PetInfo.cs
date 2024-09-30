using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using PetHouse.Domain.Shared.Constraints;
using PetHouse.Domain.Shared.Other;
using PetHouse.Domain.Shared.ValueObjects;

namespace PetHouse.Domain.PetManagment.ValueObjects;

public record PetInfo
{
    private const double MAX_WEIGHT = 120; 
    private const double MAX_HEIGHT = 150; 
    
    private PetInfo(
        string color, 
        string healthInfo,
        double weight,
        double height,
        bool isCastrated,
        bool isVaccinated,
        DateTime birthdayDate)
    {
        Color = color;
        HealthInfo = healthInfo;
        Weight = weight;
        Height = height;
        IsCastrated = isCastrated;
        IsVaccinated = isVaccinated;
        BirthdayDate = birthdayDate;
    }

    public string Color { get; private set; }

    public string HealthInfo { get; private set; }

    public double Weight { get; private set; }

    public double Height { get; private set; }

    public bool IsCastrated { get; private set; }
    
    public bool IsVaccinated { get; private set; }
    
    public DateTime BirthdayDate { get; private set; }

    public static Result<PetInfo, Error> Create(
        string color,
        string healthInfo,
        double weight,
        double height,
        bool isCastrated,
        bool isVaccinated,
        DateTime birthdayDate)
    {
        if (!Regex.IsMatch(color, @"(?:#|0x)(?:[a-f0-9]{3}|[a-f0-9]{6})\b|(?:rgb|hsl)a?\([^\)]*\)"))
        {
            return Errors.General.ValueIsInvalid("color");
        }

        if (string.IsNullOrWhiteSpace(healthInfo) ||
            healthInfo.Length > DefaultConstraints.MAX_DESCRIPTION_LENGTH)
        {
            return Errors.General.ValueIsInvalid("health_info");
        }
        
        if (double.IsNegative(weight) ||
            weight > MAX_WEIGHT)
        {
            return Errors.General.ValueIsInvalid("weight");
        }
        
        if (double.IsNegative(height) ||
            weight > MAX_HEIGHT)
        {
            return Errors.General.ValueIsInvalid("height");
        }
        
        if (Math.Abs(birthdayDate.Year - DateTime.UtcNow.Year) > 50)
        {
            return Errors.General.ValueIsInvalid("birthday_date");
        }

        return new PetInfo(color, healthInfo, weight, height, isCastrated, isVaccinated, birthdayDate);
    }
}