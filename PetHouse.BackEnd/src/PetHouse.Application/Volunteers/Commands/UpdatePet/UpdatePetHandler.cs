using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHouse.Application.Abstraction;
using PetHouse.Application.Extensions;
using PetHouse.Domain.PetManagment.ValueObjects;
using PetHouse.Domain.Shared.Id;
using PetHouse.Domain.Shared.Other;
using PetHouse.Domain.Shared.ValueObjects;

namespace PetHouse.Application.Volunteers.Commands.AddPet;

public class UpdatePetHandler : ICommandHandler<UpdatePetCommand>
{
    private readonly IVolunteersRepository _repository;
    private readonly ILogger<UpdatePetHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdatePetCommand> _validator;

    public UpdatePetHandler(IVolunteersRepository repository,
        ILogger<UpdatePetHandler> logger, 
        IUnitOfWork unitOfWork,
        IValidator<UpdatePetCommand> validator)
    {
        _repository = repository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<UnitResult<ErrorList>> Handle(UpdatePetCommand commandInput, CancellationToken cancellationToken)
    {
        var validationRes = await _validator.ValidateAsync(commandInput, cancellationToken);

        if (validationRes.IsValid == false)
        {
            return validationRes.ToList();
        }
        
        var volunteer = await _repository.GetById(commandInput.VolunteerId, cancellationToken);
        
        if (volunteer.IsFailure)
        {
            return Errors.General.NotFound().ToErrorList();
        }

        var request = commandInput.EditPetDto;

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

        var requisites = request.RequisiteDtos
            .Select(r => Requisite
                .Create(r.Name, r.Description).Value).ToList();

        var creationDate = DateTime.UtcNow.AddHours(3);

        volunteer.Value.UpdatePet(PetId.Create(commandInput.PetId),
            name,
            petIdentifier,
            description,
            petInfo,
            address,
            phoneNumber,
            requisites,
            creationDate);

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Added {Pet} to {Volunteer} successfully", name, volunteer.Value);

        return UnitResult.Success<ErrorList>();
    }
}