using PetHouse.Domain.Models.Other;

namespace PetHouse.Domain.ValueObjects;

public record SocialNetworkInfo
{
    public SocialNetworkInfo()
    {
        
    }
    public SocialNetworkInfo(ICollection<SocialNetwork> socialNetworks)
    {
        SocialNetworks = socialNetworks.ToList();
    }
    public ICollection<SocialNetwork> SocialNetworks { get; }
    
}