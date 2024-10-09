using CSharpFunctionalExtensions;
using PetHouse.SharedKernel.Id;
using PetHouse.SharedKernel.ValueObjects;
using PetHouse.SpecieManagement.Domain.Aggregate;

namespace PetHouse.SpecieManagement.Application.SpecieManagement;

public interface ISpecieRepository
{
    Task<Result<Guid, Error>> Create(Specie specie, CancellationToken cancellationToken = default);

    Task<Result<Specie, Error>> GetByName(Name name, CancellationToken cancellationToken = default);

    Task<Result<Specie, Error>> GetById(SpeciesId id, CancellationToken cancellationToken = default);

    Task<UnitResult<Error>> DeleteSpecie(SpeciesId id, CancellationToken cancellationToken = default);
}