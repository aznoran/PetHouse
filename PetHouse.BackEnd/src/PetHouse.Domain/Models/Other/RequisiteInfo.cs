using PetHouse.Domain.Models.Other;

namespace PetHouse.Domain.ValueObjects;

public record RequisiteInfo
{
    public RequisiteInfo()
    {
        
    }
    public RequisiteInfo(IEnumerable<Requisite> requisites)
    {
        Requisites = requisites.ToList();
    }
    public IReadOnlyList<Requisite> Requisites { get; }
}