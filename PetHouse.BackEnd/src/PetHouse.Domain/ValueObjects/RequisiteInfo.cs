namespace PetHouse.Domain.ValueObjects;

public record RequisiteInfo()
{
    public ICollection<Requisite> Requisites { get; }
}