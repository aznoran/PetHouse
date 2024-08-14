namespace PetHouse.Domain.ValueObjects;

public record SocialNetworkInfo
{
    public ICollection<SocialNetwork> SocialNetworks { get; }
}