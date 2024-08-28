namespace PetHouse.Domain.Models;

public record PetPhoto
{
    public string Path { get; }

    public bool IsMain { get; }
}