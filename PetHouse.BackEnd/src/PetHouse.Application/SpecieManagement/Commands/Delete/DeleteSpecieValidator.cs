using FluentValidation;
using PetHouse.Application.Validation;
using PetHouse.Domain.Shared.Other;

namespace PetHouse.Application.SpecieManagement.Commands.Delete;

public class DeleteSpecieValidator : AbstractValidator<DeleteSpecieCommand>
{
    public DeleteSpecieValidator()
    {
        RuleFor(v => v.Id).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}