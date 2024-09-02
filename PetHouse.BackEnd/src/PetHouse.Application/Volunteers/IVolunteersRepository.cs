using PetHouse.Domain.Models;
using PetHouse.Domain.Models.Volunteers.ValueObjects;

namespace PetHouse.Application.Volunteers;

public interface IVolunteersRepository
{
    Task<Guid> Create(Volunteer volunteer, CancellationToken cancellationToken = default);

    Task<Volunteer> GetByEmail(Email email, CancellationToken cancellationToken = default);
}