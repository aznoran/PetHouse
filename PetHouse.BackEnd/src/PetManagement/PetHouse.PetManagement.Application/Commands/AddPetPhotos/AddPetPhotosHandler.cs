using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetHouse.Core.Abstraction;
using PetHouse.Core.Extensions;
using PetHouse.Core.Messaging;
using PetHouse.Core.Providers;
using PetHouse.PetManagement.Domain.ValueObjects;
using PetHouse.SharedKernel.Constraints;
using PetHouse.SharedKernel.Id;
using PetHouse.SharedKernel.Other;
using PetHouse.SharedKernel.ValueObjects;
using FileInfo = PetHouse.Core.Providers.FileInfo;

namespace PetHouse.PetManagement.Application.Commands.AddPetPhotos;

public class AddPetPhotosHandler : ICommandHandler<AddPetPhotosCommand, Guid>
{
    private readonly IVolunteersRepository _repository;
    private readonly ILogger<AddPetPhotosHandler> _logger;
    private readonly IMessageQueue<IEnumerable<FileInfo>> _messageQueue;
    private readonly IFileProvider _fleprovider;
    private readonly IValidator<AddPetPhotosCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public AddPetPhotosHandler(IVolunteersRepository repository,
        ILogger<AddPetPhotosHandler> logger,
        IMessageQueue<IEnumerable<FileInfo>> messageQueue,
        IFileProvider minio,
        IValidator<AddPetPhotosCommand> validator,
        [FromKeyedServices(ModuleNames.PetManagement)]IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _logger = logger;
        _messageQueue = messageQueue;
        _fleprovider = minio;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> Handle(AddPetPhotosCommand command, CancellationToken cancellationToken)
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

        var uploadFilesRes = await _fleprovider.UploadFiles(data, cancellationToken);

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