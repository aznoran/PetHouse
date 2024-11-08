namespace PetHouse.SharedKernel.Id;

public record MessageId
{
    private MessageId(Guid value)
    {
        Value = value;
    }
    
    
    public Guid Value { get; }
    
    public static MessageId NewId => new(Guid.NewGuid());

    public static MessageId NewEmptyId => new(Guid.Empty);

    public static MessageId Create(Guid id) => new(id);
    
    public static implicit operator Guid(MessageId id)
    {
        return id.Value;
    }
}