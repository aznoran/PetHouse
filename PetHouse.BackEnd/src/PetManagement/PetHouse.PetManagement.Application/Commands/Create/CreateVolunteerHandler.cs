using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHouse.Core.Abstraction;
using PetHouse.Core.Extensions;
using PetHouse.PetManagement.Domain.Aggregate;
using PetHouse.PetManagement.Domain.ValueObjects;
using PetHouse.SharedKernel.Id;
using PetHouse.SharedKernel.Other;
using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.PetManagement.Application.Commands.Create;

public class CreateVolunteerHandler : ICommandHandler<CreateVolunteerCommand, Guid>
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
            return validationResult.ToErrorList();
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

        var requisites = command.RequisiteDto.Select(
                r => Requisite.Create(
                    r.Name,
                    r.Description)).Select(r => r.Value)
            .ToList();

        Volunteer volunteer = Volunteer.Create(
            VolunteerId.NewId,
            fullName,
            email,
            description,
            yearsOfExperience,
            phoneNumber,
            requisites).Value;

        _logger.LogInformation("Created Volunteer {FullName} with id {VolunteerId}", fullName, volunteer.Id);
        
        var res = await _repository.Create(volunteer, cancellationToken);

        await _unitOfWork.SaveChanges(cancellationToken);

        return res;
    }
}