using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHouse.Application.Abstraction;
using PetHouse.Application.Extensions;
using PetHouse.Application.Messaging;
using PetHouse.Application.Providers;
using PetHouse.Domain.PetManagment.ValueObjects;
using PetHouse.Domain.Shared.Id;
using PetHouse.Domain.Shared.Other;
using PetHouse.Domain.Shared.ValueObjects;
using FileInfo = PetHouse.Application.Providers.FileInfo;

namespace PetHouse.Application.Volunteers.Commands.AddPetPhotos;

public class AddPetPhotoHandler : ICommandHandler<AddPetPhotoCommand, Guid>
{
    private const string BUCKET_NAME = "photos";

    private readonly IVolunteersRepository _repository;
    private readonly ILogger<AddPetPhotoHandler> _logger;
    private readonly IMessageQueue<IEnumerable<FileInfo>> _messageQueue;
    private readonly IFileProvider _fleprovider;
    private readonly IValidator<AddPetPhotoCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public AddPetPhotoHandler(IVolunteersRepository repository,
        ILogger<AddPetPhotoHandler> logger,
        IMessageQueue<IEnumerable<FileInfo>> messageQueue,
        IFileProvider minio,
        IValidator<AddPetPhotoCommand> validator,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _logger = logger;
        _messageQueue = messageQueue;
        _fleprovider = minio;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<Guid, ErrorList>> Handle(AddPetPhotoCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToList();
        }

        var volunteer = await _repository.GetById(command.VolunteerId, cancellationToken);

        if (volunteer.IsFailure)
        {
            return Errors.General.NotFound().ToErrorList();
        }
        
        var extension = Path.GetExtension(command.File.FileName);

        var filePath = FilePath.Create(Guid.NewGuid(), extension);
        
        if (filePath.IsFailure)
            return filePath.Error.ToErrorList();

        var fileData = new FileData(command.File.Content, new FileInfo(filePath.Value, BUCKET_NAME));
        
        var uploadFileRes = await _fleprovider.UploadFiles([fileData], cancellationToken);

        if (uploadFileRes.IsFailure)
        {
            await _messageQueue.WriteAsync([fileData.FileInfo], cancellationToken);

            return uploadFileRes.Error.ToErrorList();
        }

        var petPhoto = PetPhoto.Create(uploadFileRes.Value.First().Path, command.IsMain);
        
        var updatePetPhotoRes = volunteer.Value
            .AddPetPhoto(PetId.Create(command.PetId),
                petPhoto.Value
        );

        if (updatePetPhotoRes.IsFailure)
            return updatePetPhotoRes.Error.ToErrorList();

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Added {Photo} to {Pet} successfully", petPhoto.Value, command.PetId);

        return command.PetId;
    }
}