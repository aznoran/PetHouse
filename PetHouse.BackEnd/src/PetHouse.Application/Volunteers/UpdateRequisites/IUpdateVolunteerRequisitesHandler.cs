using CSharpFunctionalExtensions;
using PetHouse.Domain.Shared;

namespace PetHouse.Application.Volunteers.UpdateRequisites;

public interface IUpdateVolunteerRequisitesHandler
{
    Task<Result<Guid, Error>> Handle(
        UpdateVolunteerRequisitesRequest request,
        CancellationToken cancellationToken);
}