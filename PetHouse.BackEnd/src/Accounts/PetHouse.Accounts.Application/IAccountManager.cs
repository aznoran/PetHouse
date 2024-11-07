using PetHouse.Accounts.Domain.Models;

namespace PetHouse.Accounts.Application;

public interface IAccountManager
{
    Task AddAdminAccount(AdminAccount adminAccount);
    Task AddParticipantAccount(ParticipantAccount participantAccount);
}