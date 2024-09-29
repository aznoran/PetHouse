using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHouse.Application.Abstraction;
using PetHouse.Application.Extensions;
using PetHouse.Application.Messaging;
using PetHouse.Application.Providers;
using PetHouse.Domain.PetManagment.ValueObjects;
using PetHouse.Domain.Shared.Constraints;
using PetHouse.Domain.Shared.Id;
using PetHouse.Domain.Shared.Other;
using PetHouse.Domain.Shared.ValueObjects;
using FileInfo = PetHouse.Application.Providers.FileInfo;

namespace PetHouse.Application.Volunteers.Commands.AddPetPhotos;

public class AddPetPhotosHandler : ICommandHandler<AddPetPhotosCommand, Guid>
{
    private readonly IVolunteersRepository _repository;
    private readonly ILogger<AddPetPhotosHandler> _logger;
    private readonly IMessageQueue<IEnumerable<FileInfo>> _messageQueue;
    private readonly IFileProvider _minio;
    private readonly IValidator<AddPetPhotosCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public AddPetPhotosHandler(IVolunteersRepository repository,
        ILogger<AddPetPhotosHandler> logger,
        IMessageQueue<IEnumerable<FileInfo>> messageQueue,
        IFileProvider minio,
        IValidator<AddPetPhotosCommand> validator,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _logger = logger;
        _messageQueue = messageQueue;
        _minio = minio;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<Guid, ErrorList>> Handle(AddPetPhotosCommand command, CancellationToken cancellationToken)
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

        List<FileData> data = new List<FileData>();

        foreach (var file in command.Files)
        {
            var extension = Path.GetExtension(file.FileName);

            var filePath = FilePath.Create(Guid.NewGuid(), extension);
            if (filePath.IsFailure)
                return filePath.Error.ToErrorList();

            var fileData = new FileData(file.Content, new FileInfo(filePath.Value, DefaultConstraints.BUCKET_PHOTO_NAME));

            data.Add(fileData);
        }

        var uploadFilesRes = await _minio.UploadFiles(data, cancellationToken);

        if (uploadFilesRes.IsFailure)
        {
            await _messageQueue.WriteAsync(data.Select(f => f.FileInfo), cancellationToken);

            return uploadFilesRes.Error.ToErrorList();
        }


        var addPetPhotoRes = volunteer.Value.AddPetPhotos(PetId.Create(command.PetId),
            
                uploadFilesRes.Value.Select(x =>
                    PetPhoto.Create(
                        x.Path,
                        command.IsMain).Value
                
            ).ToList()
        );

        if (addPetPhotoRes.IsFailure)
            return addPetPhotoRes.Error.ToErrorList();

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Added {Photos} to {Pet} successfully", uploadFilesRes.Value, command.PetId);

        return command.PetId;
    }
}