using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHouse.Application.Abstraction;
using PetHouse.Application.Extensions;
using PetHouse.Domain.Shared.Id;
using PetHouse.Domain.Shared.Other;
using PetHouse.Domain.Shared.ValueObjects;

namespace PetHouse.Application.Volunteers.Commands.Delete;

public class DeletePetHandler : ICommandHandler<DeletePetCommand, Guid>
{
    private readonly IVolunteersRepository _repository;
    private readonly ILogger<DeletePetHandler> _logger;
    private readonly IValidator<DeletePetCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReadDbContext _dbContext;

    public DeletePetHandler(
        IVolunteersRepository repository,
        ILogger<DeletePetHandler> logger,
        IValidator<DeletePetCommand> validator,
        IUnitOfWork unitOfWork,
        IReadDbContext dbContext)
    {
        _repository = repository;
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
    }

    public async Task<Result<Guid, ErrorList>> Handle(DeletePetCommand command,
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
        
        var pet = volunteer.Value.Pets.FirstOrDefault(p => p.Id == PetId.Create(command.PetId));

        var deletePetForceRes = volunteer.Value.DeletePetForce(pet!);

        if (deletePetForceRes.IsFailure)
        {
            return deletePetForceRes.Error.ToErrorList();
        }
        
        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Force deleted volunteer's {VolunteerId} pet with id {Id} ",
            command.VolunteerId, command.PetId);

        return command.PetId;
    }
}