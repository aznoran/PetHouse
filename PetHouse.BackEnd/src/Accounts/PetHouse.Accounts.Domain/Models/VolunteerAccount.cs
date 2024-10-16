using PetHouse.PetManagement.Domain.ValueObjects;
using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.Accounts.Domain.Models;

public class VolunteerAccount
{
    public Guid Id { get; set; }
    public User User { get; set; }
    public Guid UserId { get; set; }
    private List<Requisite> _requisites = [];
    public IReadOnlyList<Requisite> Requisites => _requisites;
    public int YearsOfExperience { get; set; }
    private List<string> _certificates = [];
    public IReadOnlyList<string> Certificates => _certificates;
    public FullName FullName { get; set; }
}