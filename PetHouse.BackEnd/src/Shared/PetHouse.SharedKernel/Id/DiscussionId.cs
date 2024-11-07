namespace PetHouse.SharedKernel.Id;

public record DiscussionId
{
    private DiscussionId(Guid value)
    {
        Value = value;
    }
    
    
    public Guid Value { get; }
    
    public static DiscussionId NewId => new(Guid.NewGuid());

    public static DiscussionId NewEmptyId => new(Guid.Empty);

    public static DiscussionId Create(Guid id) => new(id);
    
    public static implicit operator Guid(DiscussionId id)
    {
        return id.Value;
    }
}