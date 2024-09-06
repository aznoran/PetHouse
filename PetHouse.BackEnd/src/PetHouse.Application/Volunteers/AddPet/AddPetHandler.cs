using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetHouse.Application.Volunteers.Create;
using PetHouse.Domain;
using PetHouse.Domain.Enums;
using PetHouse.Domain.Models;
using PetHouse.Domain.Models.Other;
using PetHouse.Domain.Models.Volunteers.ValueObjects;
using PetHouse.Domain.Shared;
using PetHouse.Domain.ValueObjects;

namespace PetHouse.Application.Volunteers.AddPet;

public class AddPetHandler : IAddPetHandler
{
    private readonly IVolunteersRepository _repository;
    private readonly ILogger<AddPetHandler> _logger;
    public AddPetHandler(IVolunteersRepository repository, ILogger<AddPetHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<UnitResult<Error>> Handle(AddPetRequest requestInput, CancellationToken cancellationToken)
    {
        
        
        var volunteer = await _repository.GetById(requestInput.VolunteerId, cancellationToken);

        if (volunteer.IsFailure)
        {
            return Errors.General.NotFound();
        }
        
        var request = requestInput.AddPetDto;
        
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

        var address = Address.Create(request.City, request.Street).Value;

        var phoneNumber = PhoneNumber.Create(request.PhoneNumber).Value;

        var requisites = new RequisiteInfo(request.RequisiteDtos
            .Select(r => Requisite
                .Create(r.Name,r.Description).Value));

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

        await _repository.Save(volunteer.Value,cancellationToken);
        
        _logger.LogInformation("Added {Pet} to {Volunteer} successfully", name ,volunteer.Value);

        return UnitResult.Success<Error>();
    }
}