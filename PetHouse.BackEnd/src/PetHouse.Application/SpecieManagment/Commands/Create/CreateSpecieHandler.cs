using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHouse.Application.Abstraction;
using PetHouse.Application.Extensions;
using PetHouse.Application.SpecieManagment.Commands.Create;
using PetHouse.Domain.PetManagment.ValueObjects;
using PetHouse.Domain.Shared.Id;
using PetHouse.Domain.Shared.Other;
using PetHouse.Domain.Shared.ValueObjects;
using PetHouse.Domain.Specie.Entities;
using PetHouse.Infrastructure.Repositories;

namespace PetHouse.Application.SpecieManagment.Commands;

public class CreateSpecieHandler : ICommandHandler<CreateSpecieCommand, Guid>
{
    private readonly ISpecieRepository _repository;
    private readonly ILogger<CreateSpecieHandler> _logger;
    private readonly IValidator<CreateSpecieCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSpecieHandler(ISpecieRepository repository,
        ILogger<CreateSpecieHandler> logger,
        IValidator<CreateSpecieCommand> validator,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }  
    
    public async Task<Result<Guid, ErrorList>> Handle(CreateSpecieCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToErrorList();
        }

        var name = Name.Create(command.Name).Value;

        var specie = Specie.Create(SpeciesId.NewId, name);

        if (specie.IsFailure)
        {
            return specie.Error.ToErrorList();
        }

        var createSpecieResult = await _repository.Create(specie.Value, cancellationToken);

        if (createSpecieResult.IsFailure)
        {
            _logger.LogError("Specie with {Name} already exists", name);
            return createSpecieResult.Error.ToErrorList();
        }
        
        _logger.LogInformation("Created Specie {Name} with id {SpecieId}", name, specie.Value.Id);

        await _unitOfWork.SaveChanges(cancellationToken);

        return createSpecieResult.Value;
    }
}