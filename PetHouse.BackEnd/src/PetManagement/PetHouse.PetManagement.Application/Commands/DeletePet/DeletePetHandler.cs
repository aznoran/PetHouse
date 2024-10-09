using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHouse.Core.Abstraction;
using PetHouse.Core.Extensions;
using PetHouse.Core.Providers;
using PetHouse.SharedKernel.Constraints;
using PetHouse.SharedKernel.Id;
using PetHouse.SharedKernel.Other;
using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.PetManagement.Application.Commands.DeletePet;

public class DeletePetHandler : ICommandHandler<DeletePetCommand, Guid>
{
    private readonly IVolunteersRepository _repository;
    private readonly ILogger<DeletePetHandler> _logger;
    private readonly IValidator<DeletePetCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileProvider _fileProvider;

    public DeletePetHandler(
        IVolunteersRepository repository,
        ILogger<DeletePetHandler> logger,
        IValidator<DeletePetCommand> validator,
        IUnitOfWork unitOfWork,
        IFileProvider fileProvider)
    {
        _repository = repository;
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _fileProvider = fileProvider;
    }

    public async Task<Result<Guid, ErrorList>> Handle(DeletePetCommand command,
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
        
        var pet = volunteer.Value.Pets.FirstOrDefault(p => p.Id == PetId.Create(command.PetId));

        var deletePetForceRes = volunteer.Value.DeletePetForce(pet!);

        if (deletePetForceRes.IsFailure)
        {
            return deletePetForceRes.Error.ToErrorList();
        }

        var fileNames = new List<string>();
        
        foreach (var petPhoto in pet!.PetPhotos ?? [])
        {
            fileNames.Add(petPhoto.Path.Value);
        }

        var deleteFilesRes = await _fileProvider
            .DeleteFiles(DefaultConstraints.BUCKET_PHOTO_NAME, fileNames, cancellationToken);

        if (deleteFilesRes.IsFailure)
        {
            return deleteFilesRes.Error.ToErrorList();
        }
        
        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Force deleted volunteer's {VolunteerId} pet with id {Id} ",
            command.VolunteerId, command.PetId);

        return command.PetId;
    }
}