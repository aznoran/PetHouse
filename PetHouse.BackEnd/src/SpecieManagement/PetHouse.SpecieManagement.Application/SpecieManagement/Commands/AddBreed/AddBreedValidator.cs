using FluentValidation;
using PetHouse.Core.Validation;
using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.SpecieManagement.Application.SpecieManagement.Commands.AddBreed;

public class AddBreedValidator : AbstractValidator<AddBreedCommand>
{
    public AddBreedValidator()
    {
        RuleFor(c => c.Name)
            .MustBeValueObject(Name.Create);
    }
}