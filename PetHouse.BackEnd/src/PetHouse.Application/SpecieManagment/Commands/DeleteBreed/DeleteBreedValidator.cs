using FluentValidation;
using PetHouse.Application.Validation;
using PetHouse.Domain.PetManagment.ValueObjects;
using PetHouse.Domain.Shared.Other;

namespace PetHouse.Application.SpecieManagment.Commands.Create;

public class DeleteBreedValidator : AbstractValidator<DeleteBreedCommand>
{
    public DeleteBreedValidator()
    {
        RuleFor(v => v.SpecieId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(v => v.BreedId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}