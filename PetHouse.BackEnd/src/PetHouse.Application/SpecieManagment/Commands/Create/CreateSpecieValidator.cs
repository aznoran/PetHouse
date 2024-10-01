using FluentValidation;
using PetHouse.Application.Validation;
using PetHouse.Domain.Shared.ValueObjects;

namespace PetHouse.Application.SpecieManagment.Commands.Create;

public class CreateSpecieValidator : AbstractValidator<CreateSpecieCommand>
{
    public CreateSpecieValidator()
    {
        RuleFor(c => c.Name)
            .MustBeValueObject(Name.Create);
    }
}