namespace PetHouse.Domain;

public class PetPhoto
{
    public Guid Id { get; private set; }
    
    public string Path { get; private set; }
    
    public bool IsMain { get; private set; }
    
}