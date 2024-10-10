using FluentValidation;
using PetHouse.Core.Validation;
using PetHouse.SharedKernel.Other;
using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.PetManagement.Application.Commands.ChangePetMainPhoto;

public class ChangePetMainPhotoValidator : AbstractValidator<ChangePetMainPhotoCommand>
{
    public ChangePetMainPhotoValidator()
    {
        RuleFor(v => v.PetId).NotEmpty().WithError(Errors.General.ValueIsInvalid("id"));
        
        RuleFor(v => v.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsInvalid("id"));

        RuleFor(v => v.FileName).MustBeValueObject(FilePath.Create);
    }
}