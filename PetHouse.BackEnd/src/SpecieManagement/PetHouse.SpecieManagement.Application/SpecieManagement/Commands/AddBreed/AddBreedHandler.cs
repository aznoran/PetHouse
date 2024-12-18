﻿using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetHouse.Core.Abstraction;
using PetHouse.Core.Extensions;
using PetHouse.Core.Providers;
using PetHouse.SharedKernel.Constraints;
using PetHouse.SharedKernel.Id;
using PetHouse.SharedKernel.ValueObjects;
using PetHouse.SpecieManagement.Domain.Entities;

namespace PetHouse.SpecieManagement.Application.SpecieManagement.Commands.AddBreed;

public class AddBreedHandler : ICommandHandler<AddBreedCommand, Guid>
{
    private readonly ISpecieRepository _repository;
    private readonly ILogger<AddBreedHandler> _logger;
    private readonly IValidator<AddBreedCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public AddBreedHandler(ISpecieRepository repository,
        ILogger<AddBreedHandler> logger,
        IValidator<AddBreedCommand> validator,
        [FromKeyedServices(ModuleNames.SpecieManagement)]IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }  
    
    public async Task<Result<Guid, ErrorList>> Handle(AddBreedCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToErrorList();
        }

        var name = Name.Create(command.Name).Value;

        var specieRes = await _repository.GetById(SpeciesId.Create(command.Id), cancellationToken);

        if (specieRes.IsFailure)
        {
            _logger.LogError("Specie with {Id} not found", command.Id);
            return specieRes.Error.ToErrorList();
        }

        var breed = Breed.Create(BreedId.NewId, name);

        var addBreedRes = specieRes.Value.AddBreed(breed);

        if (addBreedRes.IsFailure)
        {
            _logger.LogError("Breed with {Name} already exists", name);
            return addBreedRes.Error.ToErrorList();
        }
        
        
        _logger.LogInformation("Created breed {Name}", name.Value);

        await _unitOfWork.SaveChanges(cancellationToken);

        return addBreedRes.Value;
    }
}