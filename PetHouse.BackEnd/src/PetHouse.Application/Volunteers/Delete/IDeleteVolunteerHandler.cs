using CSharpFunctionalExtensions;
using PetHouse.Domain.Shared;

namespace PetHouse.Application.Volunteers.Delete;

public interface IDeleteVolunteerHandler
{
    Task<Result<Guid, ErrorList>> Handle(DeleteVolunteerCommand command, CancellationToken cancellationToken);
}