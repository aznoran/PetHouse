using FluentValidation;
using PetHouse.Application.Validation;
using PetHouse.Domain.PetManagement.ValueObjects;
using PetHouse.Domain.Shared.Other;
using PetHouse.Domain.Shared.ValueObjects;

namespace PetHouse.Application.PetManagement.Commands.AddPet;

public class AddPetValidator : AbstractValidator<AddPetCommand>
{
    public AddPetValidator()
    {
        RuleFor(p => p.VolunteerId).NotEmpty()
            .WithError(Errors.General.ValueIsRequired("volunteer"));
        
        RuleFor(p => p.AddPetDto.Name).MustBeValueObject(Name.Create);
        
        //TODO: добавить валидацию для PETIDENTIFIER когда будут использоваться породы и виды

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
        
        RuleFor(p => p.AddPetDto.PetStatus).MustBeValueObject(PetStatusInfo.Create);

        RuleForEach(p => p.AddPetDto.RequisiteDtos)
            .MustBeValueObject(x =>
                Requisite.Create(x.Name, x.Description));
    }
}