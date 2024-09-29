using FluentValidation;
using PetHouse.Application.Validation;
using PetHouse.Domain.Shared.Other;

namespace PetHouse.Application.PetManagement.Commands.AddPetPhotos;

public class AddPetPhotosValidator : AbstractValidator<AddPetPhotosCommand>
{
    private const long MAX_PHOTO_BYTES = 10000000;
    
    public AddPetPhotosValidator()
    {
        RuleFor(v => v.PetId).NotEmpty().WithError(Errors.General.ValueIsInvalid("id"));
        
        RuleFor(v => v.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsInvalid("id"));
        
        RuleForEach(v => v.Files)
            .NotNull()
            .Must(f => f.Content.Length < MAX_PHOTO_BYTES && f.Content.Length > 0)
            .WithError(Errors.File.WrongSize());
    }
}