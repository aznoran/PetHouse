namespace PetHouse.Domain;

public record PetId : BaseId<Guid>
{
    public PetId(Guid id) : base(id)
    {
        
    }
}