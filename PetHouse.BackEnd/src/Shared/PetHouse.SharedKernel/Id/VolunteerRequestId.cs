namespace PetHouse.SharedKernel.Id;

public record VolunteerRequestId
{
    private VolunteerRequestId(Guid value)
    {
        Value = value;
    }
    
    
    public Guid Value { get; }
    
    public static VolunteerRequestId NewId => new(Guid.NewGuid());

    public static VolunteerRequestId NewEmptyId => new(Guid.Empty);

    public static VolunteerRequestId Create(Guid id) => new(id);
    
    public static implicit operator Guid(VolunteerRequestId id)
    {
        return id.Value;
    }
}