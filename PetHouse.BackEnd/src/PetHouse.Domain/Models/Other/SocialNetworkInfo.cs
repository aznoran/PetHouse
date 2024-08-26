using PetHouse.Domain.Models.Other;

namespace PetHouse.Domain.ValueObjects;

public record SocialNetworkInfo
{
    public SocialNetworkInfo()
    {
        
    }
    public SocialNetworkInfo(IEnumerable<SocialNetwork> socialNetworks)
    {
        SocialNetworks = socialNetworks.ToList();
    }
    public IReadOnlyList<SocialNetwork> SocialNetworks { get; }
    
}