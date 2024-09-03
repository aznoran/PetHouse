using CSharpFunctionalExtensions;
using PetHouse.Domain.Shared;

namespace PetHouse.Application.Volunteers.UpdateSocialNetworks;

public interface IUpdateVolunteerSocialNetworksHandler
{
    Task<Result<Guid, Error>> Handle(
        UpdateVolunteerSocialNetworksRequest request,
        CancellationToken cancellationToken);
}