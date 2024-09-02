using Microsoft.EntityFrameworkCore;
using PetHouse.Application.Volunteers;
using PetHouse.Domain.Models;
using PetHouse.Domain.Models.Volunteers.ValueObjects;

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
    
    public async Task<Volunteer> GetByEmail(Email email, CancellationToken cancellationToken = default)
    {
        return (await _dbContext.Volunteers
            .SingleOrDefaultAsync(v => v.Email == email,cancellationToken))!;
    }
}