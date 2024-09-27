using CSharpFunctionalExtensions;
using PetHouse.Domain.PetManagment.ValueObjects;
using PetHouse.Domain.Shared.Id;
using PetHouse.Domain.Shared.ValueObjects;
using PetHouse.Domain.Specie.Entities;

namespace PetHouse.Infrastructure.Repositories;

public interface ISpecieRepository
{
    Task<Result<Guid, Error>> Create(Specie specie, CancellationToken cancellationToken = default);

    Task<Result<Specie, Error>> GetByName(Name name, CancellationToken cancellationToken = default);

    Task<Result<Specie, Error>> GetById(SpeciesId id, CancellationToken cancellationToken = default);

    Task<UnitResult<Error>> DeleteSpecie(SpeciesId id, CancellationToken cancellationToken = default);
}