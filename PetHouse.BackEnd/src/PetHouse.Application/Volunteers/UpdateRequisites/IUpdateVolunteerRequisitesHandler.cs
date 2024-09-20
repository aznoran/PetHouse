using CSharpFunctionalExtensions;
using PetHouse.Domain.Shared;

namespace PetHouse.Application.Volunteers.UpdateRequisites;

public interface IUpdateVolunteerRequisitesHandler
{
    Task<Result<Guid, ErrorList>> Handle(
        UpdateVolunteerRequisitesCommand command,
        CancellationToken cancellationToken);
}