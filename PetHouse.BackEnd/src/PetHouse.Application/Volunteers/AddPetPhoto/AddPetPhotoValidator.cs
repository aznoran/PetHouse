using FluentValidation;
using PetHouse.Application.Validation;
using PetHouse.Domain.Shared;

namespace PetHouse.Application.Volunteers.AddPetPhoto;

public class AddPetPhotoValidator : AbstractValidator<AddPetPhotoRequest>
{
    private const long MAX_PHOTO_BYTES = 10000000;
    
    public AddPetPhotoValidator()
    {
        RuleFor(v => v.PetId).NotEmpty().WithError(Errors.General.ValueIsInvalid("id"));
        
        RuleFor(v => v.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsInvalid("id"));
        
        RuleForEach(v => v.Photos)
            .Must(f => f.Length < MAX_PHOTO_BYTES && f.Length > 0)
            .WithError(Errors.File.WrongSize());
    }
}