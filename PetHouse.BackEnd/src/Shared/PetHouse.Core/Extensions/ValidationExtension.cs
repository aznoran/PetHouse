using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.Core.Extensions;

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
    
    public static ErrorList ToErrorList(this IdentityResult validationResult)
    {
        var validationErrors = validationResult.Errors;

        var errors = from validationError in validationErrors
            select Error.Validation(validationError.Code, validationError.Description);

        return errors.ToList();
    }
}