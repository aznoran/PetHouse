using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHouse.Core.Abstraction;
using PetHouse.Core.Extensions;
using PetHouse.SharedKernel.Id;
using PetHouse.SharedKernel.Other;
using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.PetManagement.Application.Commands.DeletePetSoft;

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
            return validationRes.ToErrorList();
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