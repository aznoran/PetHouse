using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetHouse.Domain.Models.Other;
using PetHouse.Domain.Shared;
using PetHouse.Domain.ValueObjects;

namespace PetHouse.Application.Volunteers.UpdateSocialNetworks;

public class UpdateVolunteerSocialNetworksHandler : IUpdateVolunteerSocialNetworksHandler
{
    private readonly IVolunteersRepository _repository;
    private readonly ILogger<UpdateVolunteerSocialNetworksHandler> _logger;

    public UpdateVolunteerSocialNetworksHandler(
        IVolunteersRepository repository,
        ILogger<UpdateVolunteerSocialNetworksHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    
    public async Task<Result<Guid, Error>> Handle(
        UpdateVolunteerSocialNetworksRequest request,
        CancellationToken cancellationToken)
    {
        var volunteerResult = await _repository.GetById(request.Id, cancellationToken);

        if (volunteerResult.IsFailure)
        {
            return Errors.General.NotFound(request.Id);
        }

        var socialNetworks = request.SocialNetworksDtos
            .Select(sn => SocialNetwork
                .Create(sn.Link,sn.Name).Value)
            .ToList();

        volunteerResult.Value.UpdateSocialNetworks(new SocialNetworkInfo(socialNetworks));

        await _repository.Save(volunteerResult.Value, cancellationToken);

        _logger.LogInformation("Updated {Volunteer} social networks with id {Id}", volunteerResult.Value, request.Id);
        
        return request.Id;
    }
}