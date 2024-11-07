using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetHouse.Core.Abstraction;
using PetHouse.Core.Extensions;
using PetHouse.Core.Providers;
using PetHouse.SharedKernel.Constraints;
using PetHouse.SharedKernel.Other;
using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.PetManagement.Application.Commands.Delete;

public class DeleteVolunteerHandler : ICommandHandler<DeleteVolunteerCommand, Guid>
{
    private readonly IVolunteersRepository _repository;
    private readonly ILogger<DeleteVolunteerHandler> _logger;
    private readonly IValidator<DeleteVolunteerCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteVolunteerHandler(
        IVolunteersRepository repository,
        ILogger<DeleteVolunteerHandler> logger,
        IValidator<DeleteVolunteerCommand> validator,
        [FromKeyedServices(ModuleNames.PetManagement)]IUnitOfWork unitOfWork)
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
            return validationRes.ToErrorList();
        }
        
        var volunteer = await _repository.GetById(command.Id, cancellationToken);

        if (volunteer.IsFailure)
            return Errors.General.NotFound(command.Id).ToErrorList();

        volunteer.Value.Delete();

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Soft deleted volunteer with id {Id} and his pets", command.Id);

        return command.Id;
    }
}