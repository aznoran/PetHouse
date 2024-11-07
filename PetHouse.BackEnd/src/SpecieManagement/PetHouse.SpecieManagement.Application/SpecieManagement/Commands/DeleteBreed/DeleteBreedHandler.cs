using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetHouse.Core.Abstraction;
using PetHouse.Core.Extensions;
using PetHouse.Core.Providers;
using PetHouse.PetManagement.Contracts;
using PetHouse.SharedKernel.Constraints;
using PetHouse.SharedKernel.Id;
using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.SpecieManagement.Application.SpecieManagement.Commands.DeleteBreed;

public class DeleteBreedHandler : ICommandHandler<DeleteBreedCommand, Guid>
{
    private readonly ISpecieRepository _repository;
    private readonly ILogger<DeleteBreedHandler> _logger;
    private readonly IValidator<DeleteBreedCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPetManagementContract _contract;

    public DeleteBreedHandler(ISpecieRepository repository,
        ILogger<DeleteBreedHandler> logger,
        IValidator<DeleteBreedCommand> validator,
        [FromKeyedServices(ModuleNames.SpecieManagement)]IUnitOfWork unitOfWork,
        IPetManagementContract contract)
    {
        _repository = repository;
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _contract = contract;
    }

    public async Task<Result<Guid, ErrorList>> Handle(DeleteBreedCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToErrorList();
        }

        var findRes = await _contract
            .NotHasPetWithSpecieOrBreed(command.SpecieId, command.BreedId, cancellationToken);

        if (findRes.IsFailure)
        {
            _logger.LogError("Some pets has this specie or breed.");
            return findRes.Error.ToErrorList();
        }

        var specieRes = await _repository
            .GetById(SpeciesId.Create(command.SpecieId), cancellationToken);

        if (specieRes.IsFailure)
        {
            _logger.LogError("No specie found.");
            return specieRes.Error.ToErrorList();
        }

        var removeBreedRes = specieRes.Value.RemoveBreed(BreedId.Create(command.BreedId));

        if (removeBreedRes.IsFailure)
        {
            _logger.LogError("Error occured during removing breed.");
            return removeBreedRes.Error.ToErrorList();
        }

        _logger.LogInformation("Breed with {Id} with specieId: {specieId} was deleted.",
            command.BreedId, command.SpecieId);

        await _unitOfWork.SaveChanges(cancellationToken);

        return command.BreedId;
    }
}