using FluentValidation;
using PetHouse.Application.Validation;
using PetHouse.Domain.Shared.Other;

namespace PetHouse.Application.PetManagement.Commands.DeletePetPhoto;

public class DeletePetPhotoValidator : AbstractValidator<DeletePetPhotoCommand>
{
    private const long MAX_PHOTO_BYTES = 10000000;
    
    public DeletePetPhotoValidator()
    {
        RuleFor(v => v.PetId).NotEmpty().WithError(Errors.General.ValueIsInvalid("id"));
        
        RuleFor(v => v.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsInvalid("id"));
        
        RuleFor(v => v.FileName)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsInvalid("FileName"));
    }
}