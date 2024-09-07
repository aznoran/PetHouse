using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetHouse.Application.Volunteers;
using PetHouse.Domain.Models;
using PetHouse.Domain.Models.Volunteers.ValueObjects;
using PetHouse.Domain.Shared;
using PetHouse.Domain.ValueObjects;

namespace PetHouse.Infrastructure.Repositories;

public class VolunteersRepository : IVolunteersRepository
{
    private readonly PetHouseDbContext _dbContext;

    public VolunteersRepository(PetHouseDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> Create(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        await _dbContext.Volunteers.AddAsync(volunteer, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return volunteer.Id;
    }

    public async Task<Result<Volunteer, Error>> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var res = await _dbContext.Volunteers
            .Include(v => v.Pets)!
            .ThenInclude(p => p.PetPhotos)
            .FirstOrDefaultAsync(v => v.Id == VolunteerId.Create(id), cancellationToken);

        if (res is null)
        {
            return Errors.General.NotFound();
        }

        return res;
    }

    public async Task<Result<Volunteer, Error>> GetByEmail(Email email, CancellationToken cancellationToken = default)
    {
        var res = await _dbContext.Volunteers
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
        var res = await _dbContext.Volunteers
            .SingleOrDefaultAsync(v => v.PhoneNumber == phoneNumber, cancellationToken);

        if (res is null)
        {
            return Errors.General.NotFound();
        }

        return res;
    }

    public async Task Save(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        _dbContext.Attach(volunteer);

        var a = _dbContext.ChangeTracker.Entries<PetPhotoInfo>();
        
        var b = _dbContext.ChangeTracker.Entries<PetPhoto>();
        
        var c = _dbContext.ChangeTracker.Entries<Volunteer>();
        
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}