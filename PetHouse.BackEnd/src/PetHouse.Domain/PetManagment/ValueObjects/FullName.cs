﻿using CSharpFunctionalExtensions;
using PetHouse.Domain.Constraints;
using PetHouse.Domain.Shared;

namespace PetHouse.Domain.Models.Volunteers.ValueObjects;

public record FullName
{
    private FullName(string name, string surname)
    {
        Name = name;
        Surname = surname;
    }
    
    public string Name { get; }
    
    public string Surname { get; }

    public static Result<FullName, Error> Create(string name, string surname)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > DefaultConstraints.MAX_NAME_LENGTH)
        {
            return Result.Failure<FullName, Error>(Errors.General.ValueIsInvalid(nameof(name)));
        }
        if (string.IsNullOrWhiteSpace(surname) || surname.Length > DefaultConstraints.MAX_NAME_LENGTH)
        {
            return Result.Failure<FullName, Error>(Errors.General.ValueIsInvalid(nameof(surname)));
        }

        return new FullName(name, surname);
    }
}