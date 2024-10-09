using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHouse.Core.Abstraction;
using PetHouse.Core.Extensions;
using PetHouse.SharedKernel.Id;
using PetHouse.SharedKernel.ValueObjects;
using PetHouse.SpecieManagement.Domain.Aggregate;

namespace PetHouse.SpecieManagement.Application.SpecieManagement.Commands.Create;

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