using FluentValidation;
using PetHouse.Application.Validation;
using PetHouse.Domain.Models.Other;
using PetHouse.Domain.Shared;

namespace PetHouse.Application.Volunteers.UpdateRequisites;

public class UpdateVolunteerRequisitesValidator : AbstractValidator<UpdateVolunteerRequisitesRequest>
{
    public UpdateVolunteerRequisitesValidator()
    {
        RuleFor(v => v.Id).NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleForEach(v => v.RequisiteDto)
            .MustBeValueObject(r => Requisite.Create(r.Name, r.Description));
    }
}