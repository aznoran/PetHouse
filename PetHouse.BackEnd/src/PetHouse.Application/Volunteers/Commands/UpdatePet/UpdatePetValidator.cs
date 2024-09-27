using FluentValidation;
using PetHouse.Application.Validation;
using PetHouse.Domain.PetManagment.ValueObjects;
using PetHouse.Domain.Shared.Other;
using PetHouse.Domain.Shared.ValueObjects;

namespace PetHouse.Application.Volunteers.Commands.AddPet;

public class UpdatePetValidator : AbstractValidator<UpdatePetCommand>
{
    public UpdatePetValidator()
    {
        RuleFor(p => p.VolunteerId).NotEmpty()
            .WithError(Errors.General.ValueIsRequired("volunteer"));
        
        RuleFor(p => p.EditPetDto.Name).MustBeValueObject(Name.Create);
        
        //TODO: добавить валидацию для PETIDENTIFIER когда будут использоваться породы и виды

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