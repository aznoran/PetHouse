namespace PetHouse.Domain.Models;

public class Breed : Entity<BreedId>
{
    public string Name { get; private set; }
}