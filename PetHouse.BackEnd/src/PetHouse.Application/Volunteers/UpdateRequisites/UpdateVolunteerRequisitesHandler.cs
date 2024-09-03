using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetHouse.Domain.Models.Other;
using PetHouse.Domain.Models.Volunteers.ValueObjects;
using PetHouse.Domain.Shared;
using PetHouse.Domain.ValueObjects;

namespace PetHouse.Application.Volunteers.UpdateRequisites;

public class UpdateVolunteerRequisitesHandler : IUpdateVolunteerRequisitesHandler
{
    private readonly IVolunteersRepository _repository;
    private readonly ILogger<UpdateVolunteerRequisitesHandler> _logger;

    public UpdateVolunteerRequisitesHandler(
        IVolunteersRepository repository,
        ILogger<UpdateVolunteerRequisitesHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    
    public async Task<Result<Guid, Error>> Handle(
        UpdateVolunteerRequisitesRequest request,
        CancellationToken cancellationToken)
    {
        var volunteerResult = await _repository.GetById(request.Id, cancellationToken);

        if (volunteerResult.IsFailure)
        {
            return Errors.General.NotFound(request.Id);
        }

        var requisites = request.RequisiteDto
            .Select(r => Requisite.Create(r.Name, r.Description).Value)
            .ToList();

        volunteerResult.Value.UpdateRequisites(new RequisiteInfo(requisites));

        await _repository.Save(volunteerResult.Value, cancellationToken);

        _logger.LogInformation("Updated {Volunteer} requisites with id {Id}", volunteerResult.Value, request.Id);
        
        return request.Id;
    }
}