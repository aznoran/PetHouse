using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetHouse.SharedKernel.Other;
using PetHouse.SharedKernel.ValueObjects;
using PetHouse.SpecieManagement._Contracts;
using PetHouse.SpecieManagement.Application;

namespace PetHouse.SpecieManagement.Presentation;

public class SpecieManagementContract : ISpecieManagementContract
{
    private readonly IReadDbContext _readDbContext;

    public SpecieManagementContract(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }
    
    public async Task<UnitResult<Error>> SpecieAndBreedExist(Guid specieId, Guid breedId, CancellationToken cancellationToken)
    {
        var findSpeciesBreedsRes = await _readDbContext.Breeds
            .FirstOrDefaultAsync(b => b.Id == breedId &&
                                      b.SpecieId == specieId,
                cancellationToken: cancellationToken);
        
        if (findSpeciesBreedsRes is null)
        {
            return Errors.General.NotFound();
        }

        return UnitResult.Success<Error>();
    }
}