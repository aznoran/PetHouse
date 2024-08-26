using PetHouse.Domain.Constraints;


namespace PetHouse.Domain.Models.Other;

public record SocialNetwork
{
    public SocialNetwork()
    {
        
    }
    private SocialNetwork(string link, string name)
    {
        Link = link;
        Name = name;
    }

    public string Link { get; }


    public string Name { get; }

    public static SocialNetwork Create(
        string link, 
        string name)
    {
        if (link.Length > DefaultConstraints.MAX_LINK_LENGTH)
        {
            throw new Exception("SocialNetwork creation error : link");
        }

        if (name.Length > DefaultConstraints.MAX_NAME_LENGTH)
        {
            throw new Exception("SocialNetwork creation error : name");
        }

        return new SocialNetwork(
            link, 
            name);
    }
}