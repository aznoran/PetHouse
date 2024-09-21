﻿using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHouse.Application.Extensions;
using PetHouse.Application.Volunteers.Create;
using PetHouse.Domain.Shared;
using PetHouse.Infrastructure;

namespace PetHouse.Application.Volunteers.Delete;

public class DeleteVolunteerHandler : IDeleteVolunteerHandler
{
    private readonly IVolunteersRepository _repository;
    private readonly ILogger<DeleteVolunteerHandler> _logger;
    private readonly IValidator<DeleteVolunteerCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteVolunteerHandler(
        IVolunteersRepository repository,
        ILogger<DeleteVolunteerHandler> logger,
        IValidator<DeleteVolunteerCommand> validator,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> Handle(DeleteVolunteerCommand command,
        CancellationToken cancellationToken)
    {
        var validationRes = await _validator.ValidateAsync(command, cancellationToken);

        if (validationRes.IsValid == false)
        {
            return validationRes.ToList();
        }

       // await _unitOfWork.BeginTransaction(cancellationToken);
        
        var volunteer = await _repository.GetById(command.Id, cancellationToken);

        if (volunteer.IsFailure)
            return Errors.General.NotFound(command.Id).ToErrorList();

        volunteer.Value.Delete();

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Soft deleted volunteer with id {Id} and his pets", command.Id);

        return command.Id;
    }
}