using PetHouse.Domain.Shared;
using PetHouse.Domain.ValueObjects;

namespace PetHouse.Domain.Models;

public sealed class Volunteer : Entity<VolunteerId>
{
    public Volunteer()
    {
        
    }
    private Volunteer(VolunteerId volunteerId, VolunteerProfile volunteerProfile, SocialNetworkInfo? socialNetworks, RequisiteInfo? requisites,
        ICollection<Pet>? pets) : base(volunteerId)
    {
        VolunteerProfile = volunteerProfile;
        SocialNetworks = socialNetworks;
        Requisites = requisites;
        Pets = pets;
    }

    public VolunteerProfile VolunteerProfile { get; private set; }

    public SocialNetworkInfo? SocialNetworks { get; private set; }

    public RequisiteInfo? Requisites { get; private set; }

    public ICollection<Pet>? Pets { get; private set; }


    public static Volunteer Create(VolunteerId volunteerId,VolunteerProfile volunteerProfile, SocialNetworkInfo socialNetworks,
        RequisiteInfo requisites, ICollection<Pet>? pets)
    {
        var volunteer = new Volunteer(volunteerId,volunteerProfile, socialNetworks, requisites, pets);

        return volunteer;
    }
}