using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetHouse.Application.Abstraction;
using PetHouse.Application.Extensions;
using PetHouse.Domain.Shared.Id;
using PetHouse.Domain.Shared.Other;
using PetHouse.Domain.Shared.ValueObjects;

namespace PetHouse.Application.SpecieManagment.Commands.Delete;

public class DeleteSpecieHandler : ICommandHandler<DeleteSpecieCommand, Guid>
{
    private readonly ISpecieRepository _repository;
    private readonly ILogger<DeleteSpecieHandler> _logger;
    private readonly IValidator<DeleteSpecieCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReadDbContext _dbContext;

    public DeleteSpecieHandler(ISpecieRepository repository,
        ILogger<DeleteSpecieHandler> logger,
        IValidator<DeleteSpecieCommand> validator,
        IUnitOfWork unitOfWork,
        IReadDbContext dbContext)
    {
        _repository = repository;
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
    }

    public async Task<Result<Guid, ErrorList>> Handle(DeleteSpecieCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToErrorList();
        }

        var findRes = await _dbContext.Pets
            .FirstOrDefaultAsync(p => p.PetIdentifier.SpeciesId == command.Id,
            cancellationToken: cancellationToken);

        if (findRes is not null)
        {
            _logger.LogError("Some pets has this specie or breed.");
            return Errors.Specie.SomePetHasThisSpecieOrBreed(findRes.Id, "").ToErrorList();
        }

        await _repository.DeleteSpecie(SpeciesId.Create(command.Id), cancellationToken);

        _logger.LogInformation("Specie with {Id} was deleted.", command.Id);

        await _unitOfWork.SaveChanges(cancellationToken);

        return command.Id;
    }
}