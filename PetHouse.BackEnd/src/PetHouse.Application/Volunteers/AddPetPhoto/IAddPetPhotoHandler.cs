using CSharpFunctionalExtensions;
using PetHouse.Domain.Shared;

namespace PetHouse.Application.Volunteers.AddPetPhoto;

public interface IAddPetPhotoHandler
{
    Task<Result<IReadOnlyList<string?>, Error>> Handle(AddPetPhotoRequest request, CancellationToken cancellationToken);
}