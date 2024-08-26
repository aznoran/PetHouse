using PetHouse.Domain.Shared;
using PetHouse.Domain.ValueObjects;

namespace PetHouse.Domain.Models;

public sealed class Volunteer : Entity<VolunteerId>
{
    public Volunteer()
    {
        
    }
    private Volunteer(VolunteerId volunteerId, VolunteerProfile volunteerProfile, SocialNetworkInfo? socialNetworks, RequisiteInfo? requisites) : base(volunteerId)
    {
        VolunteerProfile = volunteerProfile;
        SocialNetworks = socialNetworks;
        Requisites = requisites;
    }

    public VolunteerProfile VolunteerProfile { get; private set; }

    public SocialNetworkInfo? SocialNetworks { get; private set; }

    public RequisiteInfo? Requisites { get; private set; }

    public ICollection<Pet>? Pets { get; private set; }


    public static Volunteer Create(
        VolunteerId volunteerId,
        VolunteerProfile volunteerProfile,
        SocialNetworkInfo socialNetworks,
        RequisiteInfo requisites)
    {
        var volunteer = new Volunteer(
            volunteerId,
            volunteerProfile,
            socialNetworks, 
            requisites);

        return volunteer;
    }
}