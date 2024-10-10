using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetHouse.PetManagement.Application;
using PetHouse.PetManagement.Contracts;
using PetHouse.SharedKernel.Other;
using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.PetManagement.Presentation;

public class PetManagementContract : IPetManagementContract
{
    private readonly IReadDbContext _readDbContext;

    public PetManagementContract(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<UnitResult<Error>> NotHasPetWithSpecie(Guid specieId, CancellationToken cancellationToken = default)
    {
        var findRes = await _readDbContext.Pets
            .FirstOrDefaultAsync(p => p.PetIdentifier.SpeciesId == specieId,
                cancellationToken: cancellationToken);

        if (findRes is not null)
        {
            return Errors.Specie.SomePetHasThisSpecieOrBreed(findRes.Id, "");
        }

        return UnitResult.Success<Error>();
    }

    public async Task<UnitResult<Error>> NotHasPetWithSpecieOrBreed(Guid specieId, Guid breedId, CancellationToken cancellationToken = default)
    {
        var findRes1 = await _readDbContext.Pets
            .FirstOrDefaultAsync(p => p.PetIdentifier.SpeciesId == specieId,
                cancellationToken: cancellationToken);

        var findRes2 = await _readDbContext.Pets
            .FirstOrDefaultAsync(p => p.PetIdentifier.BreedId == breedId,
                cancellationToken: cancellationToken);
        
        if (findRes1 is not null || findRes2 is not null)
        {
            return Errors.Specie
                .SomePetHasThisSpecieOrBreed(findRes1 is null ? findRes2.Id : findRes1.Id, "");
        }
        
        return UnitResult.Success<Error>();
    }
}