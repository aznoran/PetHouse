using PetHouse.Accounts.Domain.Models;

namespace PetHouse.Accounts.Infrastructure.Managers;

public interface IParticipantAccountManager
{
    Task AddParticipantAccount(ParticipantAccount participantAccount);
}