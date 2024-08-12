namespace PetHouse.Domain;

public class Entity
{
    public Guid Id { get; private set; }

    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }
        Entity entity = (Entity)obj!;
        return (entity.Id == Id && this == obj);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
    
    public static bool operator == (Entity entityOne, Entity entityTwo)
    {
        return object.Equals(entityOne, entityTwo);
    }
    
    public static bool operator != (Entity entityOne, Entity entityTwo)
    {
        return !object.Equals(entityOne, entityTwo);
    }
}