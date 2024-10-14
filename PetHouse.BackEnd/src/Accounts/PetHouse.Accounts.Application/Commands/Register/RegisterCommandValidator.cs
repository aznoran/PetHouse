using FluentValidation;
using PetHouse.Core.Validation;
using PetHouse.SharedKernel.Other;

namespace PetHouse.Accounts.Application.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(c => c.Email).NotEmpty().WithError(Errors.General.ValueIsInvalid("email"));
        
        RuleFor(c => c.Email).NotEmpty().WithError(Errors.General.ValueIsInvalid("password"));
    }
}