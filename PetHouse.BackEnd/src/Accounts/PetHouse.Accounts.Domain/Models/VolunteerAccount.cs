using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.Accounts.Domain.Models;

public class VolunteerAccount
{
    public Guid Id { get; set; }
    public User User { get; set; }
    private List<Requisite> _requisites = [];
    public IReadOnlyList<Requisite> Requisites => _requisites;
    public int YearsOfExperience { get; set; }
    private List<Certificate> _certificates = [];
    public IReadOnlyList<Certificate> Certificates => _certificates;
    public FullName FullName { get; set; }

    public void UpdateRequisites(IEnumerable<Requisite> requisites)
    {
        _requisites = requisites.ToList();
    }
}