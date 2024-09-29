namespace PetHouse.Domain.Shared.Id;

public record SpeciesId
{
    public SpeciesId(Guid value)
    {
        Value = value;
    }
    
    public Guid Value { get; }
    
    public static SpeciesId NewId => new(Guid.NewGuid());

    public static SpeciesId NewEmptyId => new(Guid.Empty);

    public static SpeciesId Create(Guid id) => new(id);
    
    public static implicit operator Guid(SpeciesId id)
    {
        return id.Value;
    }
}