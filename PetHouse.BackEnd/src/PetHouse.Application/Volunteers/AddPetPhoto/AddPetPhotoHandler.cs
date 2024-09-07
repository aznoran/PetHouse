using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetHouse.Application.Providers;
using PetHouse.Domain.Models;
using PetHouse.Domain.Shared;
using PetHouse.Domain.ValueObjects;

namespace PetHouse.Application.Volunteers.AddPetPhoto;

public class AddPetPhotoHandler : IAddPetPhotoHandler
{
    private readonly IVolunteersRepository _repository;
    private readonly ILogger<AddPetPhotoHandler> _logger;
    private readonly IFileProvider _minio;

    public AddPetPhotoHandler(IVolunteersRepository repository, ILogger<AddPetPhotoHandler> logger,
        IFileProvider minio)
    {
        _repository = repository;
        _repository = repository;
        _logger = logger;
        _minio = minio;
    }

    public async Task<Result<IReadOnlyList<string?>, Error>> Handle(AddPetPhotoRequest request, CancellationToken cancellationToken)
    {
        var volunteer = await _repository.GetById(request.VolunteerId, cancellationToken);

        if (volunteer.IsFailure)
        {
            return Errors.General.NotFound();
        }

        List<string?> photoLinks = new List<string?>();

        foreach (var photo in request.Photos)
        {
            await using var stream = photo.OpenReadStream();

            var fileName = Guid.NewGuid();

            var uploadRes = await _minio.Upload(stream, "photos", fileName, cancellationToken);

            if (uploadRes.IsFailure)
                return uploadRes.Error;
            
            var getRes = await _minio.Get("photos", fileName.ToString(), cancellationToken);

            if (getRes.IsFailure)
                return getRes.Error;
            
            photoLinks.Add(getRes.Value);
            
            stream.Close();
        }
        
        var addPetPhotoRes = volunteer.Value.AddPetPhoto(PetId.Create(request.PetId),
            new PetPhotoInfo(
                photoLinks.Select(x=>
                    PetPhoto.Create(
                        x!, 
                        request.IsMain).Value
                    ).ToList()
                )
            );

        if (addPetPhotoRes.IsFailure)
            return addPetPhotoRes.Error;

        await _repository.Save(volunteer.Value, cancellationToken);

        _logger.LogInformation("Added {Photos} to {Pet} successfully", photoLinks, request.PetId);

        return photoLinks;
    }
}