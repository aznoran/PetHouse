namespace PetHouse.Domain.Models;

public record PetPhoto 
{
    
    public string Path { get; private set; }
    
    public bool IsMain { get; private set; }
    
}