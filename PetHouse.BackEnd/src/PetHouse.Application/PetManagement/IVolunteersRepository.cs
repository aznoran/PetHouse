using CSharpFunctionalExtensions;
using PetHouse.Domain.PetManagment.Aggregate;
using PetHouse.Domain.PetManagment.ValueObjects;
using PetHouse.Domain.Shared.ValueObjects;

namespace PetHouse.Application.PetManagement;

public interface IVolunteersRepository
{
    Task<Guid> Create(Volunteer volunteer, CancellationToken cancellationToken = default);
  
    Task<Result<Volunteer, Error>> GetById(Guid id, CancellationToken cancellationToken = default);

    Task Save(Volunteer volunteer, CancellationToken cancellationToken = default);

    Task<Result<Volunteer, Error>> GetByPhoneNumber(PhoneNumber phoneNumber,
        CancellationToken cancellationToken = default);
  
    Task<Result<Volunteer,Error>> GetByEmail(Email email, CancellationToken cancellationToken = default);

}