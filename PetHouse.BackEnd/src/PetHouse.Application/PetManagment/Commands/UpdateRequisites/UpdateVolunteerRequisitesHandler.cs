using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHouse.Application.Abstraction;
using PetHouse.Application.Extensions;
using PetHouse.Domain.Shared.Other;
using PetHouse.Domain.Shared.ValueObjects;

namespace PetHouse.Application.Volunteers.Commands.UpdateRequisites;

public class UpdateVolunteerRequisitesHandler : ICommandHandler<UpdateVolunteerRequisitesCommand, Guid>
{
    private readonly IVolunteersRepository _repository;
    private readonly ILogger<UpdateVolunteerRequisitesHandler> _logger;
    private readonly IValidator<UpdateVolunteerRequisitesCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateVolunteerRequisitesHandler(
        IVolunteersRepository repository,
        ILogger<UpdateVolunteerRequisitesHandler> logger,
        IValidator<UpdateVolunteerRequisitesCommand> validator,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateVolunteerRequisitesCommand command,
        CancellationToken cancellationToken)
    {
        var validationRes = await _validator.ValidateAsync(command, cancellationToken);

        if (validationRes.IsValid == false)
        {
            return validationRes.ToErrorList();
        }

        var volunteerResult = await _repository.GetById(command.Id, cancellationToken);

        if (volunteerResult.IsFailure)
        {
            return Errors.General.NotFound(command.Id).ToErrorList();
        }

        var requisites = command.RequisiteDto
            .Select(r => Requisite.Create(r.Name, r.Description).Value)
            .ToList();

        volunteerResult.Value.UpdateRequisites(requisites);

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Updated {Volunteer} requisites with id {Id}", volunteerResult.Value, command.Id);

        return command.Id;
    }
}