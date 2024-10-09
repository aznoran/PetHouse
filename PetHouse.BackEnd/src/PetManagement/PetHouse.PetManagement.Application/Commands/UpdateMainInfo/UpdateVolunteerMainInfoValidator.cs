using FluentValidation;
using PetHouse.Core.Validation;
using PetHouse.PetManagement.Domain.ValueObjects;
using PetHouse.SharedKernel.Other;
using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.PetManagement.Application.Commands.UpdateMainInfo;

public class UpdateVolunteerMainInfoValidator : AbstractValidator<UpdateVolunteerMainInfoCommand>
{
    public UpdateVolunteerMainInfoValidator()
    {
        RuleFor(v => v.Id).NotEmpty().WithError(Errors.General.ValueIsInvalid());
    }
}

public class UpdateMainInfoHandlerDtoValidator : AbstractValidator<UpdateVolunteerMainInfoDto>
{
    public UpdateMainInfoHandlerDtoValidator()
    {
        RuleFor(v => v.FullNameDto).MustBeValueObject(x => FullName.Create(x.Name, x.Surname));

        RuleFor(v => v.Email).MustBeValueObject(Email.Create);
        
        RuleFor(v => v.Description).MustBeValueObject(Description.Create);

        RuleFor(v => v.YearsOfExperience).MustBeValueObject(YearsOfExperience.Create);

        RuleFor(v => v.PhoneNumber).MustBeValueObject(PhoneNumber.Create);
    }
}