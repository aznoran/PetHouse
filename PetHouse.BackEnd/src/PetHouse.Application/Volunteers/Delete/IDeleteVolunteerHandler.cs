using CSharpFunctionalExtensions;
using PetHouse.Domain.Shared;

namespace PetHouse.Application.Volunteers.Delete;

public interface IDeleteVolunteerHandler
{
    Task<Result<Guid, Error>> Handle(DeleteVolunteerRequest request, CancellationToken cancellationToken);
}