using FluentValidation;
using PetHouse.Application.Validation;
using PetHouse.Domain.Shared.ValueObjects;

namespace PetHouse.Application.SpecieManagment.Commands.AddBreed;

public class AddBreedValidator : AbstractValidator<AddBreedCommand>
{
    public AddBreedValidator()
    {
        RuleFor(c => c.Name)
            .MustBeValueObject(Name.Create);
    }
}