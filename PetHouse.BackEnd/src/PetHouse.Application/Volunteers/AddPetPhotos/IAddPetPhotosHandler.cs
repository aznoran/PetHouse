using CSharpFunctionalExtensions;
using PetHouse.Domain.Shared;

namespace PetHouse.Application.Volunteers.AddPetPhoto;

public interface IAddPetPhotosHandler
{
    Task<Result<Guid, ErrorList>> Handle(AddPetPhotosCommand command, CancellationToken cancellationToken);
}