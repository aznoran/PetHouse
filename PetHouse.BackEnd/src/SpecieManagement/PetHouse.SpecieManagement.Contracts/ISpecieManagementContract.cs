using CSharpFunctionalExtensions;
using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.SpecieManagement._Contracts;

public interface ISpecieManagementContract
{
    Task<UnitResult<Error>> SpecieAndBreedExist(Guid specieId, Guid breedId, CancellationToken cancellationToken);
}