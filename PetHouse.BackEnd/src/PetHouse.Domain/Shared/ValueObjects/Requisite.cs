using CSharpFunctionalExtensions;
using PetHouse.Domain.Shared.Constraints;
using PetHouse.Domain.Shared.Other;

namespace PetHouse.Domain.Shared.ValueObjects;

public record Requisite
{
    public Requisite()
    {
    }

    private Requisite(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public string Name { get; }
    public string Description { get; }

    public static Result<Requisite, Error> Create(
        string name,
        string description)
    {
        if (name.Length > DefaultConstraints.MAX_NAME_LENGTH || string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<Requisite, Error>(Errors.General.ValueIsRequired(nameof(name)));
        }

        if (description.Length > DefaultConstraints.MAX_DESCRIPTION_LENGTH || string.IsNullOrWhiteSpace(description))
        {
            return Result.Failure<Requisite, Error>(Errors.General.ValueIsRequired(nameof(name)));
        }

        return new Requisite(name, description);
    }   
}