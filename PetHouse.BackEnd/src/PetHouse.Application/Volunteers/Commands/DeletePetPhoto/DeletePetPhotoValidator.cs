using FluentValidation;
using PetHouse.Application.Validation;
using PetHouse.Domain.Shared.Other;

namespace PetHouse.Application.Volunteers.Commands.AddPetPhotos;

public class DeletePetPhotoValidator : AbstractValidator<DeletePetPhotoCommand>
{
    private const long MAX_PHOTO_BYTES = 10000000;
    
    public DeletePetPhotoValidator()
    {
        RuleFor(v => v.PetId).NotEmpty().WithError(Errors.General.ValueIsInvalid("id"));
        
        RuleFor(v => v.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsInvalid("id"));
        
        RuleFor(v => v.FileName).NotEmpty().WithError(Errors.General.ValueIsInvalid("FileName"));
    }
}