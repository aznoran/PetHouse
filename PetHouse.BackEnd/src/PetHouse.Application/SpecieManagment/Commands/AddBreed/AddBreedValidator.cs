using FluentValidation;
using PetHouse.Application.Validation;
using PetHouse.Domain.PetManagment.ValueObjects;

namespace PetHouse.Infrastructure.Repositories.Commands.AddBreed;

public class AddBreedValidator : AbstractValidator<AddBreedCommand>
{
    public AddBreedValidator()
    {
        RuleFor(c => c.Name)
            .MustBeValueObject(Name.Create);
    }
}