using FluentValidation;
using PetHouse.Application.Validation;
using PetHouse.Domain.Shared.Other;
using PetHouse.Domain.Shared.ValueObjects;

namespace PetHouse.Application.PetManagement.Commands.ChangePetMainPhoto;

public class ChangePetMainPhotoValidator : AbstractValidator<ChangePetMainPhotoCommand>
{
    public ChangePetMainPhotoValidator()
    {
        RuleFor(v => v.PetId).NotEmpty().WithError(Errors.General.ValueIsInvalid("id"));
        
        RuleFor(v => v.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsInvalid("id"));

        RuleFor(v => v.FileName).MustBeValueObject(FilePath.Create);
    }
}