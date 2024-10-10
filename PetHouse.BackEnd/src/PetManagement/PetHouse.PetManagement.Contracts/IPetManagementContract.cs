using CSharpFunctionalExtensions;
using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.PetManagement.Contracts;

public interface IPetManagementContract
{
    Task<UnitResult<Error>> NotHasPetWithSpecie(Guid specieId, CancellationToken cancellationToken = default);
    Task<UnitResult<Error>> NotHasPetWithSpecieOrBreed(Guid specieId , Guid breedId, CancellationToken cancellationToken = default);
}