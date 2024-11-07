using CSharpFunctionalExtensions;
using PetHouse.Discussion.Domain.Entities;
using PetHouse.SharedKernel.Id;
using PetHouse.SharedKernel.Other;
using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.Discussion.Domain.Aggregate;

public sealed class Discussion : SharedKernel.ValueObjects.Entity<DiscussionId>
{
    private Discussion(DiscussionId id, Guid relationId, List<Guid> users) : base(id)
    {
        RelationId = relationId;
        _users = users;
        IsOpen = true;
    }
    
    public Guid RelationId { get; private set; }
    public bool IsOpen { get; private set; }
    private List<Guid> _users { get; set; }
    public IReadOnlyList<Guid> Users => _users;

    private List<Message> _messages { get; set; }
    public IReadOnlyList<Message> Messages => _messages;
    
    public static Result<Discussion, Error> Create(Guid relationId, List<Guid> users)
    {
        if (users.Count < 2)
        {
            return Errors.Discussion.LessThanTwoUsers();
        }

        return Result.Success<Discussion, Error>(new Discussion(DiscussionId.NewId, relationId, users));
    }
    
    public UnitResult<Error> AddComment(Message message)
    {
        if (!_users.Contains(message.UserId))
        {
            return Errors.Discussion.NotAllowedToDiscussion();
        }

        _messages.Add(message);
        
        return UnitResult.Success<Error>();
    }
    
    public UnitResult<Error> DeleteComment(Guid userId, MessageId messageId)
    {
        var message = _messages.FirstOrDefault(x => x.Id == messageId);

        if (message is null || message.UserId != userId)
        {
            return Errors.Discussion.NotAllowedToDiscussion();
        }
        
        _messages.Remove(message);
        
        return UnitResult.Success<Error>();
    }
    
    public UnitResult<Error> EditComment(Guid userId, MessageId messageId, string text)
    {
        var message = _messages.FirstOrDefault(x => x.Id == messageId);

        if (message is null || message.UserId != userId)
        {
            return Errors.Discussion.NotAllowedToDiscussion();
        }

        message.EditMessage(text);
        
        List<Message> newMessages = new List<Message>();

        foreach (var msg in _messages)
        {
            if (msg.Id != messageId)
            {
                newMessages.Add(msg);
            }
            else
            {
                newMessages.Add(message);
            }
        }
        
        _messages = newMessages;
        
        return UnitResult.Success<Error>();
    }
    
    public UnitResult<Error> CloseDiscussion(Guid userId)
    {
        if (!_users.Contains(userId))
        {
            return Errors.Discussion.NotAllowedToDiscussion();
        }

        IsOpen = false;
        
        return UnitResult.Success<Error>();
    }
}