using FluentValidation;
using PetHouse.Application.Validation;
using PetHouse.Domain.Models.Volunteers.ValueObjects;
using PetHouse.Domain.Shared;

namespace PetHouse.Application.Volunteers.UpdateMainInfo;

public class UpdateVolunteerMainInfoValidator : AbstractValidator<UpdateVolunteerMainInfoRequest>
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