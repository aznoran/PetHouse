using CSharpFunctionalExtensions;
using PetHouse.Domain.Shared;

namespace PetHouse.Application.Volunteers.CreateVolunteer;

public interface ICreateVolunteerHandler
{
    Task<Result<Guid, Error>> Handle(CreateVolunteerRequest request, CancellationToken cancellationToken);
}