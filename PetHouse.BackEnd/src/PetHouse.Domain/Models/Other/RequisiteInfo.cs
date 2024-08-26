using PetHouse.Domain.Models.Other;

namespace PetHouse.Domain.ValueObjects;

public record RequisiteInfo
{
    public RequisiteInfo()
    {
        
    }
    public RequisiteInfo(ICollection<Requisite> requisites)
    {
        Requisites = requisites.ToList();
    }
    public ICollection<Requisite> Requisites { get; }
}