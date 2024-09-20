using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHouse.Application.Extensions;
using PetHouse.Application.Providers;
using PetHouse.Domain.Models;
using PetHouse.Domain.Models.Shared.ValueObjects;
using PetHouse.Domain.Shared;
using PetHouse.Domain.ValueObjects;
using PetHouse.Infrastructure;

namespace PetHouse.Application.Volunteers.AddPetPhoto;

public class AddPetPhotosHandler : IAddPetPhotosHandler
{
    private const string BUCKET_NAME = "photos";

    private readonly IVolunteersRepository _repository;
    private readonly ILogger<AddPetPhotosHandler> _logger;
    private readonly IFileProvider _minio;
    private readonly IValidator<AddPetPhotosCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public AddPetPhotosHandler(IVolunteersRepository repository,
        ILogger<AddPetPhotosHandler> logger,
        IFileProvider minio,
        IValidator<AddPetPhotosCommand> validator,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _logger = logger;
        _minio = minio;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    //TODO: перенести валидаторы
    public async Task<Result<Guid, ErrorList>> Handle(AddPetPhotosCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToList();
        }

        //await _unitOfWork.BeginTransaction(cancellationToken);

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

            var fileData = new FileData(file.Content, filePath.Value, BUCKET_NAME);

            data.Add(fileData);
        }

        var uploadFilesRes = await _minio.UploadFiles(data, BUCKET_NAME, cancellationToken);

        if (uploadFilesRes.IsFailure)
            return uploadFilesRes.Error.ToErrorList();
        ;

        var addPetPhotoRes = volunteer.Value.AddPetPhotos(PetId.Create(command.PetId),
            new PetPhotoInfo(
                uploadFilesRes.Value.Select(x =>
                    PetPhoto.Create(
                        x,
                        command.IsMain).Value
                ).ToList()
            )
        );

        if (addPetPhotoRes.IsFailure)
            return addPetPhotoRes.Error.ToErrorList();

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Added {Photos} to {Pet} successfully", uploadFilesRes.Value, command.PetId);

        return command.PetId;
    }
}