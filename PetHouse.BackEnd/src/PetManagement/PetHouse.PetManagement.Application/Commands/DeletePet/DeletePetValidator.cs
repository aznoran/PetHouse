using FluentValidation;
using PetHouse.Core.Validation;
using PetHouse.SharedKernel.Other;

namespace PetHouse.PetManagement.Application.Commands.DeletePet;

public class DeletePetValidator : AbstractValidator<DeletePetCommand>
{
    public DeletePetValidator()
    {
        RuleFor(v => v.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleFor(v => v.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}