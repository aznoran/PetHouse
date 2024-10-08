﻿using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using PetHouse.SharedKernel.Other;
using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.PetManagement.Domain.ValueObjects;

public record Email
{
    private Email(string value)
    {
        Value = value;
    }
    
    public string Value { get; }
    
    public static Result<Email, Error> Create(string value)
    {
        if (!Regex.IsMatch(value, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
        {
            return Result.Failure<Email, Error>(Errors.Volunteer.WrongEmail(value));
        }

        return new Email(value);
    }
}