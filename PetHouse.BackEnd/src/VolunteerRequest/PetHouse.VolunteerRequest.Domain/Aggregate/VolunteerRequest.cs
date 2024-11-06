using CSharpFunctionalExtensions;
using PetHouse.SharedKernel.Id;
using PetHouse.SharedKernel.Other;
using PetHouse.SharedKernel.ValueObjects;
using PetHouse.VolunteerRequest.Domain.Enums;

namespace PetHouse.VolunteerRequest.Domain.Aggregate;

public class VolunteerRequest : SharedKernel.ValueObjects.Entity<VolunteerRequestId>
{
    private VolunteerRequest(VolunteerRequestId id,
        Guid discussionId,
        Guid userId,
        string volunteerInfo) : base(id)
    {
        DiscussionId = discussionId;
        UserId = userId;
        VolunteerInfo = volunteerInfo;
        Status = VolunteerRequestStatus.Submitted;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid AdminId { get; private set; }
    public Guid DiscussionId { get; private set; }
    public Guid UserId { get; private set; }
    public string VolunteerInfo { get; private set; }
    public VolunteerRequestStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string RejectionComment { get; private set; }

    public static VolunteerRequest Create(Guid discussionId,
        Guid userId,
        string volunteerInfo)
    {
        return new VolunteerRequest(VolunteerRequestId.NewId, discussionId, userId, volunteerInfo);
    }

    public UnitResult<Error> TakeOnReview(Guid adminId)
    {
        if (Status != VolunteerRequestStatus.Submitted)
        {
            return Errors.General.ValueIsInvalid(nameof(VolunteerRequestStatus));
        }

        Status = VolunteerRequestStatus.OnReview;
        AdminId = adminId;
        
        return UnitResult.Success<Error>();
    }
    
    public UnitResult<Error> SendForRevision(string rejectionComment)
    {
        if (Status != VolunteerRequestStatus.OnReview)
        {
            return Errors.General.ValueIsInvalid(nameof(VolunteerRequestStatus));
        }

        Status = VolunteerRequestStatus.RevisionRequired;
        RejectionComment = rejectionComment;
        
        return UnitResult.Success<Error>();
    }
    
    public UnitResult<Error> Reject()
    {
        if (Status != VolunteerRequestStatus.OnReview)
        {
            return Errors.General.ValueIsInvalid(nameof(VolunteerRequestStatus));
        }

        Status = VolunteerRequestStatus.Rejected;
        
        return UnitResult.Success<Error>();
    }
    
    public UnitResult<Error> Approve()
    {
        if (Status != VolunteerRequestStatus.OnReview)
        {
            return Errors.General.ValueIsInvalid(nameof(VolunteerRequestStatus));
        }

        Status = VolunteerRequestStatus.Approved;
        
        return UnitResult.Success<Error>();
    }
}