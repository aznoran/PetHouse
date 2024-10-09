using FluentValidation;
using PetHouse.Core.Validation;
using PetHouse.SharedKernel.Other;

namespace PetHouse.PetManagement.Application.Commands.DeletePetSoft;

public class DeletePetSoftValidator : AbstractValidator<DeletePetSoftCommand>
{
    public DeletePetSoftValidator()
    {
        RuleFor(v => v.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleFor(v => v.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}