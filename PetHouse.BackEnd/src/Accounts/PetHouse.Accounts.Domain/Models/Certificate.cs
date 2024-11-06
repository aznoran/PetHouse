using CSharpFunctionalExtensions;
using PetHouse.SharedKernel.Constraints;
using PetHouse.SharedKernel.Other;
using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.Accounts.Domain.Models;

public record Certificate
{
    public Certificate()
    {
    }

    private Certificate(string name)
    {
        Name = name;
    }

    public string Name { get; }

    public static Result<Certificate, Error> Create(
        string name)
    {
        if (name.Length > DefaultConstraints.MAX_NAME_LENGTH || string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<Certificate, Error>(Errors.General.ValueIsRequired(nameof(name)));
        }

        return new Certificate(name);
    }   
}