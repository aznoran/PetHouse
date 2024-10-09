using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHouse.Core.Abstraction;
using PetHouse.Core.Extensions;
using PetHouse.PetManagement.Domain.Enums;
using PetHouse.PetManagement.Domain.ValueObjects;
using PetHouse.SharedKernel.Other;
using PetHouse.SharedKernel.ValueObjects;
using PetHouse.SpecieManagement._Contracts;

namespace PetHouse.PetManagement.Application.Commands.AddPet;

public class AddPetHandler : ICommandHandler<AddPetCommand>
{
    private readonly IVolunteersRepository _repository;
    private readonly ILogger<AddPetHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<AddPetCommand> _validator;
    private readonly IReadDbContext _dbContext;
    private readonly ISpecieManagementContract _contract;

    public AddPetHandler(IVolunteersRepository repository,
        ILogger<AddPetHandler> logger, 
        IUnitOfWork unitOfWork,
        IValidator<AddPetCommand> validator,
        IReadDbContext dbContext,
        ISpecieManagementContract contract)
    {
        _repository = repository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _dbContext = dbContext;
        _contract = contract;
    }

    public async Task<UnitResult<ErrorList>> Handle(AddPetCommand commandInput, CancellationToken cancellationToken)
    {
        var validationRes = await _validator.ValidateAsync(commandInput, cancellationToken);

        if (validationRes.IsValid == false)
        {
            return validationRes.ToErrorList();
        }
        
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

        var findSpeciesBreedsRes = await _contract
            .SpecieAndBreedExist(commandInput.AddPetDto.PetIdentifierDto.SpeciesId,
                commandInput.AddPetDto.PetIdentifierDto.BreedId,
                cancellationToken);

        if (findSpeciesBreedsRes.IsFailure)
        {
            return findSpeciesBreedsRes.Error.ToErrorList();
        }

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

        PetStatus petStatus = (PetStatus)request.PetStatus;

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