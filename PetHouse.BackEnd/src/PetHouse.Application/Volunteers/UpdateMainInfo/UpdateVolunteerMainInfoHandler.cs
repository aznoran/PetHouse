using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHouse.Application.Extensions;
using PetHouse.Domain.Models.Volunteers.ValueObjects;
using PetHouse.Domain.Shared;
using PetHouse.Infrastructure;

namespace PetHouse.Application.Volunteers.UpdateMainInfo;

public class UpdateVolunteerMainInfoHandler : IUpdateVolunteerMainInfoHandler
{
    private readonly IVolunteersRepository _repository;
    private readonly ILogger<UpdateVolunteerMainInfoHandler> _logger;
    private readonly IValidator<UpdateVolunteerMainInfoCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateVolunteerMainInfoHandler(
        IVolunteersRepository repository,
        ILogger<UpdateVolunteerMainInfoHandler> logger,
        IValidator<UpdateVolunteerMainInfoCommand> validator,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateVolunteerMainInfoCommand command,
        CancellationToken cancellationToken)
    {
        var validationRes = await _validator.ValidateAsync(command, cancellationToken);

        if (validationRes.IsValid == false)
        {
            validationRes.ToList();
        }

        var volunteerResult = await _repository.GetById(command.Id, cancellationToken);

        if (volunteerResult.IsFailure)
        {
            return Errors.General.NotFound(command.Id).ToErrorList();
        }

        var fullName = FullName.Create(command.UpdateVolunteerMainInfoDto.FullNameDto.Name,
            command.UpdateVolunteerMainInfoDto.FullNameDto.Surname).Value;

        var email = Email.Create(command.UpdateVolunteerMainInfoDto.Email).Value;

        var description = Description.Create(command.UpdateVolunteerMainInfoDto.Description).Value;

        var yearsOfExperience = YearsOfExperience.Create(command.UpdateVolunteerMainInfoDto.YearsOfExperience).Value;

        var phoneNumber = PhoneNumber.Create(command.UpdateVolunteerMainInfoDto.PhoneNumber).Value;

        volunteerResult.Value.UpdateMainInfo(
            fullName,
            email,
            description,
            yearsOfExperience,
            phoneNumber);

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Updated {Volunteer} with id {Id}", volunteerResult.Value, command.Id);

        return command.Id;
    }
}