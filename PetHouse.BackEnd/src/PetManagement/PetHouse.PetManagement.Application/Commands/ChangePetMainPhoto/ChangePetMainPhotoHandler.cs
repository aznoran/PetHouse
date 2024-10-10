using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHouse.Core.Abstraction;
using PetHouse.Core.Extensions;
using PetHouse.SharedKernel.Id;
using PetHouse.SharedKernel.Other;
using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.PetManagement.Application.Commands.ChangePetMainPhoto;

public class ChangePetMainPhotoHandler : ICommandHandler<ChangePetMainPhotoCommand, Guid>
{
    private readonly IVolunteersRepository _repository;
    private readonly ILogger<ChangePetMainPhotoHandler> _logger;
    private readonly IValidator<ChangePetMainPhotoCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public ChangePetMainPhotoHandler(IVolunteersRepository repository,
        ILogger<ChangePetMainPhotoHandler> logger,
        IValidator<ChangePetMainPhotoCommand> validator,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<Guid, ErrorList>> Handle(ChangePetMainPhotoCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToErrorList();
        }

        var volunteer = await _repository.GetById(command.VolunteerId, cancellationToken);

        if (volunteer.IsFailure)
        {
            return Errors.General.NotFound().ToErrorList();
        }

        var changeMainPhotoRes =
            volunteer.Value.ChangePetMainPhoto(PetId.Create(command.PetId), FilePath.Create(command.FileName).Value);

        if (changeMainPhotoRes.IsFailure)
        {
            return changeMainPhotoRes.Error.ToErrorList();
        }
        
        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Changed {Photo}'s IsMain to \"main\" successfully", command.FileName);

        return command.PetId;
    }
}