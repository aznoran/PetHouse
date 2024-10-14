using FluentValidation;
using PetHouse.Core.Validation;
using PetHouse.SharedKernel.Other;

namespace PetHouse.Accounts.Application.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(c => c.Email).NotEmpty().WithError(Errors.General.ValueIsInvalid("email"));
        
        RuleFor(c => c.Email).NotEmpty().WithError(Errors.General.ValueIsInvalid("password"));
    }
}