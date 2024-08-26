using PetHouse.Domain.Constraints;

namespace PetHouse.Domain.Models.Other;

public class Requisite
{
    public Requisite()
    {
        
    }
    private Requisite(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public string Name { get; }
    public string Description { get; }

    public static Requisite Create(string name, string description)
    {
        if (name.Length > DefaultConstraints.MAX_NAME_LENGTH)
        {
            throw new Exception("Requisite creation error : name");
        }

        if (description.Length > DefaultConstraints.MAX_DESCRIPTION_LENGTH)
        {
            throw new Exception("Requisite creation error : description");
        }

        return new Requisite(name, description);
    }
}