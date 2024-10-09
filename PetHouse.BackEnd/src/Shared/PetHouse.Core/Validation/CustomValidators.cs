using CSharpFunctionalExtensions;
using FluentValidation;
using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.Core.Validation;

public static class CustomValidators
{
    public static IRuleBuilderOptionsConditions<T, TElement> MustBeValueObject<T, TElement, TValueObject>(
        this IRuleBuilder<T, TElement> ruleBuilder,
        Func<TElement, Result<TValueObject, Error>> factoryMethod)
    {
        return ruleBuilder.Custom((valueObject, context) =>
        {
            Result<TValueObject, Error> result = factoryMethod(valueObject);

            if (result.IsSuccess) return;

            context.AddFailure(result.Error.Serialize());
        });
    }

    public static IRuleBuilderOptions<T, TElement> WithError<T, TElement>(
        this IRuleBuilderOptions<T, TElement> ruleBuilder,
        Error error)
    {
        return ruleBuilder.WithMessage(error.Serialize());
    }
}