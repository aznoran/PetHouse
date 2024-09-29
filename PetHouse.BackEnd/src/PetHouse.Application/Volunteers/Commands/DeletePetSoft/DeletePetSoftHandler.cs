using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHouse.Application.Abstraction;
using PetHouse.Application.Extensions;
using PetHouse.Domain.Shared.Id;
using PetHouse.Domain.Shared.Other;
using PetHouse.Domain.Shared.ValueObjects;

namespace PetHouse.Application.Volunteers.Commands.Delete;

public class DeletePetSoftHandler : ICommandHandler<DeletePetSoftCommand, Guid>
{
    private readonly IVolunteersRepository _repository;
    private readonly ILogger<DeletePetSoftHandler> _logger;
    private readonly IValidator<DeletePetSoftCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReadDbContext _dbContext;

    public DeletePetSoftHandler(
        IVolunteersRepository repository,
        ILogger<DeletePetSoftHandler> logger,
        IValidator<DeletePetSoftCommand> validator,
        IUnitOfWork unitOfWork,
        IReadDbContext dbContext)
    {
        _repository = repository;
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
    }

    public async Task<Result<Guid, ErrorList>> Handle(DeletePetSoftCommand command,
        CancellationToken cancellationToken)
    {
        var validationRes = await _validator.ValidateAsync(command, cancellationToken);

        if (validationRes.IsValid == false)
        {
            return validationRes.ToList();
        }
        
        var volunteer = await _repository.GetById(command.VolunteerId, cancellationToken);

        if (volunteer.IsFailure)
            return Errors.General.NotFound(command.VolunteerId).ToErrorList();

        if (volunteer.Value.Pets == null)
        {
            return Errors.General.ValueIsRequired("pets").ToErrorList();
        }

        var deletePetForceRes = volunteer.Value.DeletePetSoft(PetId.Create(command.PetId));

        if (deletePetForceRes.IsFailure)
        {
            return deletePetForceRes.Error.ToErrorList();
        }
        
        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Soft deleted volunteer's {VolunteerId} pet with id {Id} ",
            command.VolunteerId, command.PetId);

        return command.PetId;
    }
}