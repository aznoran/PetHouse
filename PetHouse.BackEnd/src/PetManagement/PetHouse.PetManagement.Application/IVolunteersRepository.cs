using CSharpFunctionalExtensions;
using PetHouse.PetManagement.Domain.Aggregate;
using PetHouse.PetManagement.Domain.ValueObjects;
using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.PetManagement.Application;

public interface IVolunteersRepository
{
    Task<Guid> Create(Volunteer volunteer, CancellationToken cancellationToken = default);
  
    Task<Result<Volunteer, Error>> GetById(Guid id, CancellationToken cancellationToken = default);

    Task Save(Volunteer volunteer, CancellationToken cancellationToken = default);

    Task<Result<Volunteer, Error>> GetByPhoneNumber(PhoneNumber phoneNumber,
        CancellationToken cancellationToken = default);
  
    Task<Result<Volunteer,Error>> GetByEmail(Email email, CancellationToken cancellationToken = default);

}