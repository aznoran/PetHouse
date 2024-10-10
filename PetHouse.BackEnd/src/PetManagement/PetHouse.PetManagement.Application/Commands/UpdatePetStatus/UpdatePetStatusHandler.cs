using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHouse.Core.Abstraction;
using PetHouse.Core.Extensions;
using PetHouse.SharedKernel.Id;
using PetHouse.SharedKernel.Other;
using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.PetManagement.Application.Commands.UpdatePetStatus;

public class UpdatePetStatusHandler : ICommandHandler<UpdatePetStatusCommand>
{
    private readonly IVolunteersRepository _repository;
    private readonly ILogger<UpdatePetStatusHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdatePetStatusCommand> _validator;

    public UpdatePetStatusHandler(IVolunteersRepository repository,
        ILogger<UpdatePetStatusHandler> logger, 
        IUnitOfWork unitOfWork,
        IValidator<UpdatePetStatusCommand> validator)
    {
        _repository = repository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<UnitResult<ErrorList>> Handle(UpdatePetStatusCommand commandInput, CancellationToken cancellationToken)
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

        var petStatus = commandInput.PetStatus;

        var updatePetStatusRes = volunteer.Value
            .UpdatePetStatus(PetId.Create(commandInput.PetId), commandInput.PetStatus);

        if (updatePetStatusRes.IsFailure)
        {
            return updatePetStatusRes.Error.ToErrorList();
        }

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Updated {Pet}'s status", commandInput.PetId);

        return UnitResult.Success<ErrorList>();
    }
}