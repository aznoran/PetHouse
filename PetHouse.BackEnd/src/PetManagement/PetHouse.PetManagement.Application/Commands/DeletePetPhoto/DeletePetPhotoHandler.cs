using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetHouse.Core.Abstraction;
using PetHouse.Core.Extensions;
using PetHouse.Core.Providers;
using PetHouse.PetManagement.Domain.ValueObjects;
using PetHouse.SharedKernel.Constraints;
using PetHouse.SharedKernel.Id;
using PetHouse.SharedKernel.Other;
using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.PetManagement.Application.Commands.DeletePetPhoto;

public class DeletePetPhotoHandler : ICommandHandler<DeletePetPhotoCommand, Guid>
{
    private const string BUCKET_NAME = "photos";

    private readonly IVolunteersRepository _repository;
    private readonly ILogger<DeletePetPhotoHandler> _logger;
    private readonly IFileProvider _fleprovider;
    private readonly IValidator<DeletePetPhotoCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public DeletePetPhotoHandler(IVolunteersRepository repository,
        ILogger<DeletePetPhotoHandler> logger,
        IFileProvider minio,
        IValidator<DeletePetPhotoCommand> validator,
        [FromKeyedServices(ModuleNames.PetManagement)]IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _logger = logger;
        _fleprovider = minio;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<Guid, ErrorList>> Handle(DeletePetPhotoCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToErrorList();
        }

        var volunteer = await _repository.GetById(command.VolunteerId, cancellationToken);

        if (volunteer.IsFailure)
        {
            return Errors.General.NotFound().ToErrorList();
        }
        
        var extension = Path.GetExtension(command.FileName);

        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(command.FileName);

        var filePath = FilePath.Create(Guid.Parse(fileNameWithoutExtension), extension);
        
        if (filePath.IsFailure)
            return filePath.Error.ToErrorList();
        
        var removePetPhotoRes = volunteer.Value
            .RemovePetPhoto(PetId.Create(command.PetId),
                PetPhoto.Create(filePath.Value, false).Value);

        if (removePetPhotoRes.IsFailure)
        {
            return removePetPhotoRes.Error.ToErrorList();
        }
        
        var deleteFileRes = await _fleprovider.Delete(command.BucketName, command.FileName, cancellationToken);

        if (deleteFileRes.IsFailure)
        {
            return deleteFileRes.Error.ToErrorList();
        }

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Deleted {Photo} from {Pet} successfully", command.FileName, command.PetId);

        return command.PetId;
    }
}