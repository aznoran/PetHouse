using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHouse.Application.Extensions;
using PetHouse.Domain.Models;
using PetHouse.Domain.Models.Other;
using PetHouse.Domain.Models.Volunteers.ValueObjects;
using PetHouse.Domain.Shared;
using PetHouse.Domain.ValueObjects;
using PetHouse.Infrastructure;

namespace PetHouse.Application.Volunteers.Create;

public class CreateVolunteerHandler : ICreateVolunteerHandler
{
    private readonly IVolunteersRepository _repository;
    private readonly ILogger<CreateVolunteerHandler> _logger;
    private readonly IValidator<CreateVolunteerCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public CreateVolunteerHandler(IVolunteersRepository repository,
        ILogger<CreateVolunteerHandler> logger,
        IValidator<CreateVolunteerCommand> validator,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }   

    public async Task<Result<Guid, ErrorList>> Handle(CreateVolunteerCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToList();
        }

        //await _unitOfWork.BeginTransaction(cancellationToken);
        
        var fullName = FullName.Create(command.FullNameDto.Name, command.FullNameDto.Surname).Value;

        var description = Description.Create(command.Description).Value;

        var yearsOfExperience = YearsOfExperience.Create(command.YearsOfExperience).Value;

        var email = Email.Create(command.Email).Value;

        var emailRes = await _repository.GetByEmail(email, cancellationToken);
        
        if (emailRes.IsSuccess)
        {
            return Errors.Volunteer.AlreadyExists(email.Value, nameof(email)).ToErrorList();
        }
        
        var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;

        var phoneNumberRes = await _repository.GetByPhoneNumber(phoneNumber, cancellationToken);
        
        if (phoneNumberRes.IsSuccess)
        {
            return Errors.Volunteer.AlreadyExists(phoneNumber.Value, nameof(phoneNumber)).ToErrorList();
        }
       
        var socialNetworks = new SocialNetworkInfo(command.SocialNetworksDto
            .Select(sn => SocialNetwork.Create(sn.Link, sn.Name))
            .ToList().Select(sn => sn.Value));

        var requisites = new RequisiteInfo(command.RequisiteDto.Select(
                r => Requisite.Create(
                    r.Name,
                    r.Description)).Select(r => r.Value)
            .ToList());

        Volunteer volunteer = Volunteer.Create(
            VolunteerId.NewId,
            fullName,
            email,
            description,
            yearsOfExperience,
            phoneNumber,
            socialNetworks,
            requisites).Value;

        _logger.LogInformation("Created Volunteer {FullName} with id {VolunteerId}", fullName, volunteer.Id);
        
        var res = await _repository.Create(volunteer, cancellationToken);

        await _unitOfWork.SaveChanges(cancellationToken);

        return res;
    }
}