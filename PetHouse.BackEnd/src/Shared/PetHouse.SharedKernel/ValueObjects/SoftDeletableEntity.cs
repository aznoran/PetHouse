namespace PetHouse.SharedKernel.ValueObjects;

public abstract class SoftDeletableEntity<Tid> where Tid : notnull
{
    public Tid Id { get; }

    public bool IsDeleted { get; protected set; } = false;
    public DateTime? DeletedTime { get; protected set; } = null;

    protected SoftDeletableEntity(Tid id)
    {
        Id = id;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Entity<Tid> other)
            return false;

        if (ReferenceEquals(this, other) == false)
            return false;

        if (GetType() != other.GetType())
            return false;

        return EqualityComparer<Tid>.Equals(this, obj);
    }

    public override int GetHashCode()
    {
        return (GetType().ToString() + Id).GetHashCode();
    }

    public static bool operator ==(SoftDeletableEntity<Tid>? first, Entity<Tid>? second)
    {
        if (first is null && second is null)
            return true;

        if (first is null || second is null)
            return false;

        return first.Equals(second);
    }

    public static bool operator !=(SoftDeletableEntity<Tid>? first, Entity<Tid>? second)
    {
        return !(first == second);
    }

    public virtual void Delete()
    {
        IsDeleted = true;
        DeletedTime = DateTime.UtcNow;
    }
    
    public virtual void Restore()
    {
        IsDeleted = false;
        DeletedTime = null;
    }
}