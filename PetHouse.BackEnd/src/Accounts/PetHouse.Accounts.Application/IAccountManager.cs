using PetHouse.Accounts.Domain.Models;

namespace PetHouse.Accounts.Infrastructure.Managers;

public interface IAccountManager
{
    Task AddAdminAccount(AdminAccount adminAccount);
    Task AddParticipantAccount(ParticipantAccount participantAccount);
}