using CSharpFunctionalExtensions;
using PetHouse.Domain.Constraints;
using PetHouse.Domain.Enums;
using PetHouse.Domain.Models.Other;
using PetHouse.Domain.Models.Volunteers.ValueObjects;
using PetHouse.Domain.Shared;
using PetHouse.Domain.ValueObjects;

namespace PetHouse.Domain.Models;

public sealed class Volunteer : Shared.Entity<VolunteerId>, ISoftDeletable
{
    private bool _isDeleted = false;
    public Volunteer()
    {
    }

    private Volunteer(VolunteerId volunteerId,
        FullName fullName, 
        Email email,
        Description description,
        YearsOfExperience yearsOfExperience,
        PhoneNumber phoneNumber,
        SocialNetworkInfo? socialNetworks,
        RequisiteInfo? requisites) : base(volunteerId)
    {
        FullName = fullName;
        Email = email;
        Description = description;
        YearsOfExperience = yearsOfExperience;
        PhoneNumber = phoneNumber;
        SocialNetworks = socialNetworks;
        Requisites = requisites;
    }

    public FullName FullName { get; private set; }
    
    public Email Email { get; private set; }
    
    public Description Description { get; private set; }

    public YearsOfExperience YearsOfExperience { get; private set; }
    
    public PhoneNumber PhoneNumber { get; private set; }
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
        FullName fullName, 
        Email email,
        Description description,
        YearsOfExperience yearsOfExperience,
        PhoneNumber phoneNumber,
        SocialNetworkInfo socialNetworks,
        RequisiteInfo requisites)
    {
        var volunteer = new Volunteer(
            volunteerId,
            fullName,
            email,
            description,
            yearsOfExperience,
            phoneNumber,
            socialNetworks,
            requisites);

        return volunteer;
    }

    public void Delete()
    {
        _isDeleted = true;
        foreach (var pet in _pets)
        {
            pet.Delete();
        }
    }

    public void Restore()
    {
        _isDeleted = false;
        foreach (var pet in _pets)
        {
            pet.Restore();
        }
    }

    public void UpdateMainInfo(
        FullName fullName,
        Email email,
        Description description,
        YearsOfExperience yearsOfExperience,
        PhoneNumber phoneNumber)
    {
        FullName = fullName;
        Email = email;
        Description = description;
        YearsOfExperience = yearsOfExperience;
        PhoneNumber = phoneNumber;
    }
    
    public void UpdateRequisites(
        RequisiteInfo requisites)
    {
        Requisites = requisites;
    }
    
    public void UpdateSocialNetworks(
        SocialNetworkInfo socialNetworks)
    {
        SocialNetworks = socialNetworks;
    }
}