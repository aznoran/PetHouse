﻿using FluentValidation.Results;
using PetHouse.Domain.Shared;
using PetHouse.Domain.Shared.ValueObjects;

namespace PetHouse.Application.Extensions;

public static class ValidationExtension
{
    public static ErrorList ToErrorList(this ValidationResult validationResult)
    {
        var validationErrors = validationResult.Errors;

        var errors = from validationError in validationErrors
            let errorMessage = validationError.ErrorMessage
            let error = Error.Deserialize(errorMessage)
            select Error.Validation(error.Code, error.Message, validationError.PropertyName);

        return errors.ToList();
    }
}