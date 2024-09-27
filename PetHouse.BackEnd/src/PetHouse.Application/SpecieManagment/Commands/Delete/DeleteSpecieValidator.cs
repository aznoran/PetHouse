using FluentValidation;
using PetHouse.Application.Validation;
using PetHouse.Domain.PetManagment.ValueObjects;
using PetHouse.Domain.Shared.Other;

namespace PetHouse.Application.SpecieManagment.Commands.Create;

public class DeleteSpecieValidator : AbstractValidator<DeleteSpecieCommand>
{
    public DeleteSpecieValidator()
    {
        RuleFor(v => v.Id).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}