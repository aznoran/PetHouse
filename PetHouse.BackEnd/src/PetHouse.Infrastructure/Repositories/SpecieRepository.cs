using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetHouse.Application.SpecieManagment;
using PetHouse.Domain.PetManagment.ValueObjects;
using PetHouse.Domain.Shared.Id;
using PetHouse.Domain.Shared.Other;
using PetHouse.Domain.Shared.ValueObjects;
using PetHouse.Domain.Specie.Aggregate;
using PetHouse.Domain.Specie.Entities;
using PetHouse.Infrastructure.Data;

namespace PetHouse.Infrastructure.Repositories;

public class SpecieRepository : ISpecieRepository
{
    private readonly PetHouseWriteDbContext _writeDbContext;

    public SpecieRepository(PetHouseWriteDbContext writeDbContext)
    {
        _writeDbContext = writeDbContext;
    }

    public async Task<Result<Guid, Error>> Create(Specie specie, CancellationToken cancellationToken = default)
    {
        var findRes = await GetByName(specie.Name, cancellationToken);

        if (findRes.IsSuccess)
        {
            return Errors.Specie.AlreadyExists();
        }
        
        await _writeDbContext.Species.AddAsync(specie, cancellationToken);

        return specie.Id.Value;
    }
    
    public async Task<Result<Specie, Error>> GetByName(Name name, CancellationToken cancellationToken = default)
    {
        var res = await _writeDbContext.Species.
            SingleOrDefaultAsync(s => s.Name.Value == name.Value, cancellationToken: cancellationToken);

        if (res is null)
        {
            return Errors.General.NotFound();
        }

        return res;
    }
    
    public async Task<Result<Specie, Error>> GetById(SpeciesId id, CancellationToken cancellationToken = default)
    {
        var res = await _writeDbContext.Species.
            Include(s => s.Breeds).
            SingleOrDefaultAsync(s => s.Id == id, cancellationToken: cancellationToken);

        if (res is null)
        {
            return Errors.General.NotFound();
        }

        return res;
    }
    
    public async Task<UnitResult<Error>> DeleteSpecie(SpeciesId id, CancellationToken cancellationToken = default)
    {
        var res = await _writeDbContext.Species.
            Include(s => s.Breeds).
            SingleOrDefaultAsync(s => s.Id == id, cancellationToken: cancellationToken);
        
        if (res is null)
        {
            return Errors.General.NotFound();
        }
        
        _writeDbContext.Species.Remove(res);

        return UnitResult.Success<Error>();
    }
    
}