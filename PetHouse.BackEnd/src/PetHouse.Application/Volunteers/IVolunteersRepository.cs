using PetHouse.Domain.Models;

namespace PetHouse.Application.Volunteers;

public interface IVolunteersRepository
{
    Task<Guid> Create(Volunteer volunteer, CancellationToken cancellationToken = default);
}