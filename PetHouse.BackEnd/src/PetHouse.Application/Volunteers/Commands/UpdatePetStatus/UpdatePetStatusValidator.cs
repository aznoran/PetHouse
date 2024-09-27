using FluentValidation;
using PetHouse.Application.Validation;
using PetHouse.Domain.PetManagment.ValueObjects;
using PetHouse.Domain.Shared.Other;
using PetHouse.Domain.Shared.ValueObjects;

namespace PetHouse.Application.Volunteers.Commands.AddPet;

public class UpdatePetStatusValidator : AbstractValidator<UpdatePetStatusCommand>
{
    public UpdatePetStatusValidator()
    {
        RuleFor(p => p.VolunteerId).NotEmpty()
            .WithError(Errors.General.ValueIsRequired("volunteer"));
        
        RuleFor(p => p.PetId).NotEmpty()
            .WithError(Errors.General.ValueIsRequired("pet"));
        
        RuleFor(p => p.PetStatus).NotEmpty()
            .WithError(Errors.General.ValueIsRequired("petstatus"));
    }
}