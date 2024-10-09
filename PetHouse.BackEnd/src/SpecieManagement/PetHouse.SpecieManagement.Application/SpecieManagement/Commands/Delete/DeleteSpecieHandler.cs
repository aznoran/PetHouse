using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHouse.Core.Abstraction;
using PetHouse.Core.Extensions;
using PetHouse.PetManagement.Contracts;
using PetHouse.SharedKernel.Id;
using PetHouse.SharedKernel.Other;
using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.SpecieManagement.Application.SpecieManagement.Commands.Delete;

public class DeleteSpecieHandler : ICommandHandler<DeleteSpecieCommand, Guid>
{
    private readonly ISpecieRepository _repository;
    private readonly ILogger<DeleteSpecieHandler> _logger;
    private readonly IValidator<DeleteSpecieCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPetManagementContract _contract;

    public DeleteSpecieHandler(ISpecieRepository repository,
        ILogger<DeleteSpecieHandler> logger,
        IValidator<DeleteSpecieCommand> validator,
        IUnitOfWork unitOfWork,
        IPetManagementContract contract)
    {
        _repository = repository;
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _contract = contract;
    }

    public async Task<Result<Guid, ErrorList>> Handle(DeleteSpecieCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToErrorList();
        }

        var findRes = await _contract.NotHasPetWithSpecie(command.Id, cancellationToken);

        if (findRes.IsFailure)
        {
            _logger.LogError("Some pets has this specie or breed.");
            return findRes.Error.ToErrorList();
        }

        await _repository.DeleteSpecie(SpeciesId.Create(command.Id), cancellationToken);

        _logger.LogInformation("Specie with {Id} was deleted.", command.Id);

        await _unitOfWork.SaveChanges(cancellationToken);

        return command.Id;
    }
}