namespace PetHouse.Domain;

public record VolunteerId : BaseId<Guid>
{
    public VolunteerId(Guid id) : base(id)
    {
        
    }
}