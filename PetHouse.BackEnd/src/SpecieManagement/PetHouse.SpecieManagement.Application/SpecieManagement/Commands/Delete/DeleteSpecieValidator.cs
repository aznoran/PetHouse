using FluentValidation;
using PetHouse.Core.Validation;
using PetHouse.SharedKernel.Other;

namespace PetHouse.SpecieManagement.Application.SpecieManagement.Commands.Delete;

public class DeleteSpecieValidator : AbstractValidator<DeleteSpecieCommand>
{
    public DeleteSpecieValidator()
    {
        RuleFor(v => v.Id).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}