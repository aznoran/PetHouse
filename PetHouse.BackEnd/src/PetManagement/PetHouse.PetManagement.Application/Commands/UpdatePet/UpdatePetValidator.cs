using FluentValidation;
using PetHouse.Core.Validation;
using PetHouse.PetManagement.Domain.ValueObjects;
using PetHouse.SharedKernel.Other;
using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.PetManagement.Application.Commands.UpdatePet;

public class UpdatePetValidator : AbstractValidator<UpdatePetCommand>
{
    public UpdatePetValidator()
    {
        RuleFor(p => p.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("volunteer"));
        
        RuleFor(p => p.EditPetDto.Name).MustBeValueObject(Name.Create);
        
        RuleFor(p => p.EditPetDto.PetIdentifierDto.SpeciesId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("species_id"));
        
        RuleFor(p => p.EditPetDto.PetIdentifierDto.SpeciesId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("breed_id"));

        RuleFor(p => p.EditPetDto.Description).MustBeValueObject(Description.Create);

        RuleFor(p => p).MustBeValueObject(x =>
            PetInfo.Create(
                x.EditPetDto.Color,
                x.EditPetDto.HealthInfo,
                x.EditPetDto.Weight,
                x.EditPetDto.Height,
                x.EditPetDto.IsCastrated,
                x.EditPetDto.IsVaccinated,
                x.EditPetDto.BirthdayDate));

        RuleFor(p => p).MustBeValueObject(x =>
            Address.Create(
                x.EditPetDto.City,
                x.EditPetDto.Street,
                x.EditPetDto.Country));

        RuleFor(p => p.EditPetDto.PhoneNumber).MustBeValueObject(PhoneNumber.Create);

        RuleForEach(p => p.EditPetDto.RequisiteDtos)
            .MustBeValueObject(x =>
                Requisite.Create(x.Name, x.Description));
    }
}