using CSharpFunctionalExtensions;
using PetHouse.Domain.Shared;

namespace PetHouse.Application.Volunteers.AddPet;

public interface IAddPetHandler
{
    Task<UnitResult<ErrorList>> Handle(AddPetCommand command, CancellationToken cancellationToken);
}