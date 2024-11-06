using FluentValidation;
using PetHouse.Accounts.Application.Commands.Login;
using PetHouse.Core.Validation;
using PetHouse.SharedKernel.Other;

namespace PetHouse.Accounts.Application.Commands.Refresh;

public class RefreshCommandValidator : AbstractValidator<RefreshCommand>
{
    public RefreshCommandValidator()
    {
        RuleFor(c => c.RefreshToken)
            .NotEmpty().WithError(Errors.General.ValueIsInvalid("refresh_token"));
    }
}