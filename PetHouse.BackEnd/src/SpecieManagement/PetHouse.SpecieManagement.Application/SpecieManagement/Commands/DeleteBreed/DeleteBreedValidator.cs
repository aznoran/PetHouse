using FluentValidation;
using PetHouse.Core.Validation;
using PetHouse.SharedKernel.Other;

namespace PetHouse.SpecieManagement.Application.SpecieManagement.Commands.DeleteBreed;

public class DeleteBreedValidator : AbstractValidator<DeleteBreedCommand>
{
    public DeleteBreedValidator()
    {
        RuleFor(v => v.SpecieId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(v => v.BreedId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}