using CSharpFunctionalExtensions;
using PetHouse.Domain.Models;
using PetHouse.Domain.Models.Volunteers.ValueObjects;
using PetHouse.Domain.Shared;

namespace PetHouse.Application.Volunteers;

public interface IVolunteersRepository
{
    Task<Guid> Create(Volunteer volunteer, CancellationToken cancellationToken = default);
    Task<Result<Volunteer, Error>> GetById(Guid id, CancellationToken cancellationToken = default);

    Task<Result<Volunteer, Error>> GetByPhoneNumber(PhoneNumber phoneNumber,
        CancellationToken cancellationToken = default);
    Task Save(Volunteer volunteer, CancellationToken cancellationToken = default);
    Task<Result<Volunteer,Error>> GetByEmail(Email email, CancellationToken cancellationToken = default);
}