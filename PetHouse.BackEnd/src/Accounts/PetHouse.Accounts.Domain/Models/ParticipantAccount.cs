using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.Accounts.Domain.Models;

public class ParticipantAccount
{
    public const string PARTICIPANT = "Participant";
    public Guid Id { get; set; }
    public User User { get; set; }

    public FullName FullName { get; set; }
}