using FluentValidation;
using PetHouse.Application.Validation;
using PetHouse.Domain.Shared.Other;

namespace PetHouse.Application.Volunteers.Commands.Delete;

public class DeletePetValidator : AbstractValidator<DeletePetCommand>
{
    public DeletePetValidator()
    {
        RuleFor(v => v.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleFor(v => v.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}