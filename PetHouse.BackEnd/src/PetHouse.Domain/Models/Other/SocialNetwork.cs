using PetHouse.Domain.Constraints;


namespace PetHouse.Domain.Models.Other;

public record SocialNetwork
{
    public SocialNetwork()
    {
        
    }
    private SocialNetwork(string reference, string name)
    {
        Reference = reference;
        Name = name;
    }

    public string Reference { get; }


    public string Name { get; }

    public static SocialNetwork Create(string reference, string description)
    {
        if (reference.Length > DefaultConstraints.MAX_REFERENCE_LENGTH)
        {
            throw new Exception("SocialNetwork creation error : reference");
        }

        if (description.Length > DefaultConstraints.MAX_DESCRIPTION_LENGTH)
        {
            throw new Exception("SocialNetwork creation error : description");
        }

        return new SocialNetwork(reference, description);
    }
}