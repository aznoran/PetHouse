using CSharpFunctionalExtensions;
using PetHouse.Domain.Shared;

namespace PetHouse.Application.Volunteers.UpdateMainInfo;

public interface IUpdateVolunteerMainInfoHandler
{
    Task<Result<Guid, Error>> Handle(
        UpdateVolunteerMainInfoRequest request,
        CancellationToken cancellationToken);
}