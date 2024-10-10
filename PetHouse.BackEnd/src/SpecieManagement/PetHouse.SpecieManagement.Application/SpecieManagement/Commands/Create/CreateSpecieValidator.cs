using FluentValidation;
using PetHouse.Core.Validation;
using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.SpecieManagement.Application.SpecieManagement.Commands.Create;

public class CreateSpecieValidator : AbstractValidator<CreateSpecieCommand>
{
    public CreateSpecieValidator()
    {
        RuleFor(c => c.Name)
            .MustBeValueObject(Name.Create);
    }
}