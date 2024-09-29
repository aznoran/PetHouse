using FluentValidation;
using PetHouse.Application.Validation;
using PetHouse.Domain.Shared.Other;

namespace PetHouse.Application.PetManagment.Commands.Delete;

public class DeleteVolunteerValidator : AbstractValidator<DeleteVolunteerCommand>
{
    public DeleteVolunteerValidator()
    {
        RuleFor(v => v.Id).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}