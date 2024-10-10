using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetHouse.PetManagement.Application;
using PetHouse.PetManagement.Domain.Aggregate;
using PetHouse.PetManagement.Domain.ValueObjects;
using PetHouse.PetManagement.Infrastructure.Data;
using PetHouse.SharedKernel.Id;
using PetHouse.SharedKernel.Other;
using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.PetManagement.Infrastructure.Repositories;

public class VolunteersRepository : IVolunteersRepository
{
    private readonly PetHouseWriteDbContext _writeDbContext;

    public VolunteersRepository(PetHouseWriteDbContext writeDbContext)
    {
        _writeDbContext = writeDbContext;
    }

    public async Task<Guid> Create(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        await _writeDbContext.Volunteers.AddAsync(volunteer, cancellationToken);

        return volunteer.Id;
    }

    public async Task<Result<Volunteer, Error>> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var res = await _writeDbContext.Volunteers
            .Include(v => v.Pets)!
            .FirstOrDefaultAsync(v => v.Id == VolunteerId.Create(id), cancellationToken);

        if (res is null)
        {
            return Errors.General.NotFound();
        }

        return res;
    }

    public async Task<Result<Volunteer, Error>> GetByEmail(Email email, CancellationToken cancellationToken = default)
    {
        var res = await _writeDbContext.Volunteers
            .SingleOrDefaultAsync(v => v.Email == email, cancellationToken);

        if (res is null)
        {
            return Errors.General.NotFound();
        }

        return res;
    }

    public async Task<Result<Volunteer, Error>> GetByPhoneNumber(PhoneNumber phoneNumber,
        CancellationToken cancellationToken = default)
    {
        var res = await _writeDbContext.Volunteers
            .SingleOrDefaultAsync(v => v.PhoneNumber == phoneNumber, cancellationToken);

        if (res is null)
        {
            return Errors.General.NotFound();
        }

        return res;
    }

    public async Task Save(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        _writeDbContext.Volunteers.Attach(volunteer);
        
        await _writeDbContext.SaveChangesAsync(cancellationToken);
    }
}