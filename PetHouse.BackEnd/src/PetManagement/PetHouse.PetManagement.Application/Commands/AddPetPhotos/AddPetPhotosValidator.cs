using FluentValidation;
using PetHouse.Core.Validation;
using PetHouse.SharedKernel.Other;

namespace PetHouse.PetManagement.Application.Commands.AddPetPhotos;

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