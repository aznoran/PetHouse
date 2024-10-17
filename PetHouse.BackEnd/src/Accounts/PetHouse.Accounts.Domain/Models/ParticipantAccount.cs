using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.Accounts.Domain.Models;

public class ParticipantAccount
{
    public const string PARTICIPANT_ACCOUNTS = "Participant";
    public Guid Id { get; set; }
    public User User { get; set; }
    public Guid UserId { get; set; }

    public FullName FullName { get; set; }
}