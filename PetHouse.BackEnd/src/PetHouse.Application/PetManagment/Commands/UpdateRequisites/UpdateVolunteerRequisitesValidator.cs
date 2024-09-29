using FluentValidation;
using PetHouse.Application.Validation;
using PetHouse.Domain.Shared.Other;
using PetHouse.Domain.Shared.ValueObjects;

namespace PetHouse.Application.PetManagment.Commands.UpdateRequisites;

public class UpdateVolunteerRequisitesValidator : AbstractValidator<UpdateVolunteerRequisitesCommand>
{
    public UpdateVolunteerRequisitesValidator()
    {
        RuleFor(v => v.Id).NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleForEach(v => v.RequisiteDto)
            .MustBeValueObject(r => Requisite.Create(r.Name, r.Description));
    }
}