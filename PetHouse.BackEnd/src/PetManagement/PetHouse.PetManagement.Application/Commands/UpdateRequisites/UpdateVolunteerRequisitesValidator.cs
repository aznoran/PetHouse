using FluentValidation;
using PetHouse.Core.Validation;
using PetHouse.SharedKernel.Other;
using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.PetManagement.Application.Commands.UpdateRequisites;

public class UpdateVolunteerRequisitesValidator : AbstractValidator<UpdateVolunteerRequisitesCommand>
{
    public UpdateVolunteerRequisitesValidator()
    {
        RuleFor(v => v.Id).NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleForEach(v => v.RequisiteDto)
            .MustBeValueObject(r => Requisite.Create(r.Name, r.Description));
    }
}