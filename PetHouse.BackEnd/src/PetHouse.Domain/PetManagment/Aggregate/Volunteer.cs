using CSharpFunctionalExtensions;
using PetHouse.Domain.Enums;
using PetHouse.Domain.Models.Volunteers.ValueObjects;
using PetHouse.Domain.Shared;
using PetHouse.Domain.ValueObjects;

namespace PetHouse.Domain.Models;

public sealed class Volunteer : Domain.Shared.Entity<VolunteerId>, ISoftDeletable
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
    public IReadOnlyList<Pet>? Pets => _pets;


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

    public UnitResult<Error> AddPet(Name name,
        PetIdentifier petIdentifier,
        Description description,
        PetInfo petInfo,
        Address address,
        PhoneNumber phoneNumber,
        RequisiteInfo requisites,
        PetStatus petStatus,
        DateTime creationDate)
    {
        var positionRes = Position.Create(_pets.Count+1);

        if (positionRes.IsFailure) return positionRes.Error;
        
        var pet = new Pet(
            PetId.NewId,
            name,
            petIdentifier,
            positionRes.Value,
            description,
            petInfo,
            address,
            phoneNumber,
            requisites,
            petStatus,
            creationDate);

        _pets.Add(pet);

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> MovePet(PetId petId, Position newPosition)
    {
        var pet = _pets.Find(p => p.Id == petId);

        if (pet is null) return Errors.General.NotFound(petId.Value);
        
        var currentPos = pet.Position;

        if (currentPos == newPosition) return UnitResult.Success<Error>();
        
        if (currentPos.Value > newPosition.Value)
        {
            var petsToMove = _pets.Where(p =>
                p.Position.Value < currentPos.Value &&
                p.Position.Value >= newPosition.Value);

            foreach (var petToMove in petsToMove)
            {
                var moveRes = petToMove.MoveForward();

                if (moveRes.IsFailure) return moveRes.Error;
            }
        }
        else if (currentPos.Value < newPosition.Value)
        {
            var petsToMove = _pets.Where(p =>
                p.Position.Value > currentPos.Value &&
                p.Position.Value <= newPosition.Value);

            foreach (var petToMove in petsToMove)
            {
                var moveRes = petToMove.MoveBack();
                
                if (moveRes.IsFailure) return moveRes.Error;
            }
        }
        
        var petMoveRes = pet.Move(newPosition);
        
        if (petMoveRes.IsFailure) return petMoveRes.Error;

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> MovePetToFirstPosition(PetId petId)
    {
        var moveRes = MovePet(petId, Position.Create(1).Value);

        if (moveRes.IsFailure) return moveRes.Error;

        return UnitResult.Success<Error>();
    }
    
    public UnitResult<Error> MovePetToLastPosition(PetId petId)
    {
        var moveRes = MovePet(petId, Position.Create(_pets.Count).Value);

        if (moveRes.IsFailure) return moveRes.Error;

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> AddPetPhotos(PetId petId, PetPhotoInfo petPhotoInfo)
    {
        if (Pets is null)
            return Errors.General.NotFound();

        var pet = Pets.FirstOrDefault(p => p.Id == petId);

        if (pet is null)
            return Errors.General.NotFound(petId.Value);

        var addPhotosRes = pet.AddPhotos(petPhotoInfo);

        if (addPhotosRes.IsFailure)
            return addPhotosRes.Error;

        return UnitResult.Success<Error>();
    }
}