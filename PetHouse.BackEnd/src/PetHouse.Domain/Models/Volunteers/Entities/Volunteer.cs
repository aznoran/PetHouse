using CSharpFunctionalExtensions;
using PetHouse.Domain.Enums;
using PetHouse.Domain.Shared;
using PetHouse.Domain.ValueObjects;

namespace PetHouse.Domain.Models;

public sealed class Volunteer : Shared.Entity<VolunteerId>
{
    public Volunteer()
    {
    }

    private Volunteer(VolunteerId volunteerId, VolunteerProfile volunteerProfile, SocialNetworkInfo? socialNetworks,
        RequisiteInfo? requisites) : base(volunteerId)
    {
        VolunteerProfile = volunteerProfile;
        SocialNetworks = socialNetworks;
        Requisites = requisites;
    }

    public VolunteerProfile VolunteerProfile { get; private set; }

    public int CountOfPetsFoundHome()
    {
        return _pets.Count(p => p.PetStatus == PetStatus.FoundHome);
    }

    public int CountOfPetsLookingForHome()
    {
        return _pets.Count(p => p.PetStatus == PetStatus.LookingForHome);
    }

    public int CountOfPetsOnTreatment()
    {
        return _pets.Count(p => p.PetStatus == PetStatus.OnTreatment);
    }

    public SocialNetworkInfo? SocialNetworks { get; private set; }

    public RequisiteInfo? Requisites { get; private set; }

    private List<Pet> _pets = [];
    public ICollection<Pet>? Pets => _pets;


    public static Result<Volunteer, Error> Create(
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