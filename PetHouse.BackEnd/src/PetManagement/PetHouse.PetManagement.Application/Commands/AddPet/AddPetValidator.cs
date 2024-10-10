using FluentValidation;
using PetHouse.Core.Validation;
using PetHouse.PetManagement.Domain.Enums;
using PetHouse.PetManagement.Domain.ValueObjects;
using PetHouse.SharedKernel.Other;
using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.PetManagement.Application.Commands.AddPet;

public class AddPetValidator : AbstractValidator<AddPetCommand>
{
    public AddPetValidator()
    {
        RuleFor(p => p.VolunteerId).NotEmpty()
            .WithError(Errors.General.ValueIsRequired("volunteer"));
        
        RuleFor(p => p.AddPetDto.Name).MustBeValueObject(Name.Create);
        
        RuleFor(p => p.AddPetDto.PetIdentifierDto.SpeciesId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("species_id"));
        
        RuleFor(p => p.AddPetDto.PetIdentifierDto.SpeciesId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("breed_id"));

        RuleFor(p => p.AddPetDto.Description).MustBeValueObject(Description.Create);

        RuleFor(p => p).MustBeValueObject(x =>
            PetInfo.Create(
                x.AddPetDto.Color,
                x.AddPetDto.HealthInfo,
                x.AddPetDto.Weight,
                x.AddPetDto.Height,
                x.AddPetDto.IsCastrated,
                x.AddPetDto.IsVaccinated,
                x.AddPetDto.BirthdayDate));

        RuleFor(p => p).MustBeValueObject(x =>
            Address.Create(
                x.AddPetDto.City,
                x.AddPetDto.Street,
                x.AddPetDto.Country));

        RuleFor(p => p.AddPetDto.PhoneNumber).MustBeValueObject(PhoneNumber.Create);
        
        RuleFor(p => p.AddPetDto.PetStatus)
            .NotEmpty()
            .Must(petStatus => Enum.IsDefined(typeof(PetStatus), (PetStatus)petStatus))
            .WithError(Errors.General.ValueIsInvalid("pet-status"));

        RuleForEach(p => p.AddPetDto.RequisiteDtos)
            .MustBeValueObject(x =>
                Requisite.Create(x.Name, x.Description));
    }
}