using FluentValidation;
using PetHouse.Core.Validation;
using PetHouse.PetManagement.Domain.Enums;
using PetHouse.SharedKernel.Other;

namespace PetHouse.PetManagement.Application.Commands.UpdatePetStatus;

public class UpdatePetStatusValidator : AbstractValidator<UpdatePetStatusCommand>
{
    public UpdatePetStatusValidator()
    {
        RuleFor(p => p.VolunteerId).NotEmpty()
            .WithError(Errors.General.ValueIsRequired("volunteer"));
        
        RuleFor(p => p.PetId).NotEmpty()
            .WithError(Errors.General.ValueIsRequired("pet"));
        
        RuleFor(p => p.PetStatus)
            .NotEmpty()
            .Must(petStatus => Enum.IsDefined(typeof(PetStatus), (PetStatus)petStatus))
            .WithError(Errors.General.ValueIsInvalid("pet-status"));
    }
}