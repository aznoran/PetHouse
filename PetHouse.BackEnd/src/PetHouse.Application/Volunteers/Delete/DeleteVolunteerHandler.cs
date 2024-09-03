using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetHouse.Application.Volunteers.Create;
using PetHouse.Domain.Shared;

namespace PetHouse.Application.Volunteers.Delete;

public class DeleteVolunteerHandler : IDeleteVolunteerHandler
{
    private readonly IVolunteersRepository _repository;
    private readonly ILogger<DeleteVolunteerHandler> _logger;

    public DeleteVolunteerHandler(
        IVolunteersRepository repository,
        ILogger<DeleteVolunteerHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    
    public async Task<Result<Guid, Error>> Handle(DeleteVolunteerRequest request, CancellationToken cancellationToken)
    {
        var volunteer = await _repository.GetById(request.Id, cancellationToken);

        if (volunteer.IsFailure)
            return Errors.General.NotFound(request.Id);

        volunteer.Value.Delete();
        
        await _repository.Save(volunteer.Value, cancellationToken);

        _logger.LogInformation("Soft deleted volunteer with id {Id} and his pets", request.Id);
        
        return request.Id;
    }
}