using CSharpFunctionalExtensions;
using PetHouse.Domain.Models.Other;
using PetHouse.Domain.Shared;

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