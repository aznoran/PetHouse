using PetHouse.SharedKernel.Id;
using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.Discussion.Domain.Entities;

public class Message : Entity<MessageId>
{
    private Message(MessageId id, string text, Guid userId) : base(id)
    {
        Text = text;
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
        IsEdited = false;
    }
    
    public string Text { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsEdited { get; private set; }
    public Guid UserId { get; private set; }

    public static Message Create(string text, Guid userId)
    {
        return new Message(MessageId.NewId, text, userId);
    }

    public void EditMessage(string text)
    {
        IsEdited = true;
        Text = text;
    }
}