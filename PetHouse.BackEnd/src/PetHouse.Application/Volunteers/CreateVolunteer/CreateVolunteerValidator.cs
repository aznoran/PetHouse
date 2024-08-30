using FluentValidation;
using FluentValidation.AspNetCore;
using PetHouse.Application.Validation;
using PetHouse.Domain.Models;
using PetHouse.Domain.Models.Other;
using PetHouse.Domain.Models.Volunteers.ValueObjects;

namespace PetHouse.Application.Volunteers.CreateVolunteer;

public class CreateVolunteerValidator : AbstractValidator<CreateVolunteerRequest>
{
    public CreateVolunteerValidator()
    {
        RuleFor(v => v.FullNameDto).MustBeValueObject(x => FullName.Create(x.Name, x.Surname));

        RuleFor(v => v.Description).MustBeValueObject(Description.Create);

        RuleFor(v => v.YearsOfExperience).MustBeValueObject(YearsOfExperience.Create);

        RuleFor(v => v.PhoneNumber).MustBeValueObject(PhoneNumber.Create);

        RuleForEach(v => v.RequisiteDto)
            .MustBeValueObject(x =>
                Requisite.Create(
                    x.Name,
                    x.Description));

        RuleForEach(v => v.SocialNetworksDto)
            .MustBeValueObject(x =>
                SocialNetwork.Create(
                    x.Link,
                    x.Name));
    }
}