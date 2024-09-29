using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetHouse.Application.Abstraction;
using PetHouse.Application.Extensions;
using PetHouse.Domain.Shared.Id;
using PetHouse.Domain.Shared.Other;
using PetHouse.Domain.Shared.ValueObjects;

namespace PetHouse.Application.SpecieManagment.Commands.DeleteBreed;

public class DeleteBreedHandler : ICommandHandler<DeleteBreedCommand, Guid>
{
    private readonly ISpecieRepository _repository;
    private readonly ILogger<DeleteBreedHandler> _logger;
    private readonly IValidator<DeleteBreedCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReadDbContext _dbContext;

    public DeleteBreedHandler(ISpecieRepository repository,
        ILogger<DeleteBreedHandler> logger,
        IValidator<DeleteBreedCommand> validator,
        IUnitOfWork unitOfWork,
        IReadDbContext dbContext)
    {
        _repository = repository;
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
    }

    public async Task<Result<Guid, ErrorList>> Handle(DeleteBreedCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToErrorList();
        }

        var findRes1 = await _dbContext.Pets
            .FirstOrDefaultAsync(p => p.PetIdentifier.SpeciesId == command.SpecieId,
            cancellationToken: cancellationToken);

        var findRes2 = await _dbContext.Pets
            .FirstOrDefaultAsync(p => p.PetIdentifier.BreedId == command.BreedId,
            cancellationToken: cancellationToken);

        if (findRes1 is not null || findRes2 is not null)
        {
            _logger.LogError("Some pets has this specie or breed.");
            return Errors.Specie
                .SomePetHasThisSpecieOrBreed(findRes1 is null ? findRes2.Id : findRes1.Id, "")
                .ToErrorList();
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