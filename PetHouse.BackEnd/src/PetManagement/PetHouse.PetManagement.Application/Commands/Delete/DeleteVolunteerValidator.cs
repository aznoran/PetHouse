using FluentValidation;
using PetHouse.Core.Validation;
using PetHouse.SharedKernel.Other;

namespace PetHouse.PetManagement.Application.Commands.Delete;

public class DeleteVolunteerValidator : AbstractValidator<DeleteVolunteerCommand>
{
    public DeleteVolunteerValidator()
    {
        RuleFor(v => v.Id).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}