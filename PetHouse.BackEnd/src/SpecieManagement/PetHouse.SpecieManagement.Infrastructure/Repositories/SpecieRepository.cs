using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetHouse.SharedKernel.Id;
using PetHouse.SharedKernel.Other;
using PetHouse.SharedKernel.ValueObjects;
using PetHouse.SpecieManagement.Application.SpecieManagement;
using PetHouse.SpecieManagement.Domain.Aggregate;
using PetHouse.SpecieManagement.Infrastructure.Data;

namespace PetHouse.SpecieManagement.Infrastructure.Repositories;

public class SpecieRepository : ISpecieRepository
{
    private readonly WriteDbContext _writeDbContext;

    public SpecieRepository(WriteDbContext writeDbContext)
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