using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHouse.Application.Abstraction;
using PetHouse.Application.Extensions;
using PetHouse.Application.Volunteers.Create;
using PetHouse.Domain;
using PetHouse.Domain.Enums;
using PetHouse.Domain.Models;
using PetHouse.Domain.Models.Other;
using PetHouse.Domain.Models.Volunteers.ValueObjects;
using PetHouse.Domain.Shared;
using PetHouse.Domain.ValueObjects;
using PetHouse.Infrastructure;

namespace PetHouse.Application.Volunteers.AddPet;

public class AddPetHandler : ICommandHandler<AddPetCommand>
{
    private readonly IVolunteersRepository _repository;
    private readonly ILogger<AddPetHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<AddPetCommand> _validator;

    public AddPetHandler(IVolunteersRepository repository,
        ILogger<AddPetHandler> logger, 
        IUnitOfWork unitOfWork,
        IValidator<AddPetCommand> validator)
    {
        _repository = repository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<UnitResult<ErrorList>> Handle(AddPetCommand commandInput, CancellationToken cancellationToken)
    {
        var validationRes = await _validator.ValidateAsync(commandInput, cancellationToken);

        if (validationRes.IsValid == false)
        {
            return validationRes.ToList();
        }
        
        //await _unitOfWork.BeginTransaction(cancellationToken);
        
        var volunteer = await _repository.GetById(commandInput.VolunteerId, cancellationToken);
        
        if (volunteer.IsFailure)
        {
            return Errors.General.NotFound().ToErrorList();
        }

        var request = commandInput.AddPetDto;

        var name = Name.Create(request.Name).Value;

        var petIdentifier = PetIdentifier
            .Create(request.PetIdentifierDto.SpeciesId,
                request.PetIdentifierDto.BreedId).Value;

        var description = Description.Create(request.Description).Value;

        var petInfo = PetInfo.Create(
            request.Color,
            request.HealthInfo,
            request.Weight,
            request.Height,
            request.IsCastrated,
            request.IsVaccinated,
            request.BirthdayDate).Value;

        var address = Address.Create(request.City, request.Street, request.Country).Value;

        var phoneNumber = PhoneNumber.Create(request.PhoneNumber).Value;

        var requisites = new RequisiteInfo(request.RequisiteDtos
            .Select(r => Requisite
                .Create(r.Name, r.Description).Value));

        var petStatus = request.PetStatus;

        var creationDate = DateTime.UtcNow.AddHours(3);

        volunteer.Value.AddPet(name,
            petIdentifier,
            description,
            petInfo,
            address,
            phoneNumber,
            requisites,
            petStatus,
            creationDate);

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Added {Pet} to {Volunteer} successfully", name, volunteer.Value);

        return UnitResult.Success<ErrorList>();
    }
}