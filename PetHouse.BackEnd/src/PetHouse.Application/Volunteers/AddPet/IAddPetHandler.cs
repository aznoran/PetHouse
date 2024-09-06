using CSharpFunctionalExtensions;
using PetHouse.Domain.Shared;

namespace PetHouse.Application.Volunteers.AddPet;

public interface IAddPetHandler
{
    Task<UnitResult<Error>> Handle(AddPetRequest request, CancellationToken cancellationToken);
}