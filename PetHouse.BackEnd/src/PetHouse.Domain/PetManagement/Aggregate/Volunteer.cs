using CSharpFunctionalExtensions;
using PetHouse.Domain.PetManagement.Entities;
using PetHouse.Domain.PetManagement.Enums;
using PetHouse.Domain.PetManagement.ValueObjects;
using PetHouse.Domain.Shared.Id;
using PetHouse.Domain.Shared.Other;
using PetHouse.Domain.Shared.ValueObjects;

namespace PetHouse.Domain.PetManagement.Aggregate;

public sealed class Volunteer : Shared.ValueObjects.Entity<VolunteerId>, ISoftDeletable
{
    private bool _isDeleted = false;

    //EF CORE
    // ReSharper disable once UnusedMember.Local
    private Volunteer(VolunteerId id) : base(id){ }

    private Volunteer(VolunteerId volunteerId,
        FullName fullName,
        Email email,
        Description description,
        YearsOfExperience yearsOfExperience,
        PhoneNumber phoneNumber,
        IReadOnlyList<SocialNetwork>? socialNetworks,
        IReadOnlyList<Requisite>? requisites) : base(volunteerId)
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

    public Description Description { get; private set; }
    public Email Email { get; private set; }

    public YearsOfExperience YearsOfExperience { get; private set; }

    public PhoneNumber PhoneNumber { get; private set; }

    public int CountOfPetsFoundHome()
    {
        return _pets.Count(p => p.PetStatus.Value == PetStatus.FoundHome);
    }

    public int CountOfPetsLookingForHome()
    {
        return _pets.Count(p => p.PetStatus.Value == PetStatus.LookingForHome);
    }

    public int CountOfPetsOnTreatment()
    {
        return _pets.Count(p => p.PetStatus.Value == PetStatus.OnTreatment);
    }

    public IReadOnlyList<SocialNetwork> SocialNetworks { get; private set; }

    public IReadOnlyList<Requisite> Requisites { get; private set; }

    private List<Pet> _pets = [];
    public IReadOnlyList<Pet>? Pets => _pets;


    public static Result<Volunteer, Error> Create(
        VolunteerId volunteerId,
        FullName fullName,
        Email email,
        Description description,
        YearsOfExperience yearsOfExperience,
        PhoneNumber phoneNumber,
        IReadOnlyList<SocialNetwork> socialNetworks,
        IReadOnlyList<Requisite> requisites)
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

    public void DeleteSoft()
    {
        _isDeleted = true;
        foreach (var pet in _pets)
        {
            pet.DeleteSoft();
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
    
    public UnitResult<Error> DeletePetSoft(PetId petId)
    {
        var pet = _pets.FirstOrDefault(p => p.Id == petId);

        if (pet is null)
        {
            return Errors.General.NotFound(petId.Value);
        }
        
        pet.DeleteSoft();

        return UnitResult.Success<Error>();
    }
    
    public UnitResult<Error> DeletePetForce(Pet pet)
    {
        var removeRes = _pets.Remove(pet);

        if (removeRes == false)
        {
            return Errors.General.ValueIsInvalid();
        }

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> RestorePet(PetId petId)
    {
        var pet = _pets.FirstOrDefault(p => p.Id == petId);

        if (pet is null)
        {
            return Errors.General.NotFound(petId.Value);
        }
        
        pet.Restore();

        return UnitResult.Success<Error>();
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
        IReadOnlyList<Requisite> requisites)
    {
        Requisites = requisites;
    }

    public void UpdateSocialNetworks(
        IReadOnlyList<SocialNetwork> socialNetworks)
    {
        SocialNetworks = socialNetworks;
    }

    public UnitResult<Error> AddPet(Name name,
        PetIdentifier petIdentifier,
        Description description,
        PetInfo petInfo,
        Address address,
        PhoneNumber phoneNumber,
        IReadOnlyList<Requisite> requisites,
        PetStatusInfo petStatus,
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
    
    public UnitResult<Error> UpdatePet(PetId petId,
        Name name,
        PetIdentifier petIdentifier,
        Description description,
        PetInfo petInfo,
        Address address,
        PhoneNumber phoneNumber,
        IReadOnlyList<Requisite> requisites,
        DateTime creationDate)
    {
        var pet = _pets.FirstOrDefault(p => p.Id == petId);

        if (pet is null)
        {
            return Errors.General.NotFound();
        }

        var updateRes = pet.Update(name,
            petIdentifier,
            description,
            petInfo,
            address,
            phoneNumber,
            requisites,
            creationDate);

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> UpdatePetStatus(PetId petId, int petStatus)
    {
        var petStatusOriginal = PetStatusInfo.Create(petStatus);

        if (petStatusOriginal.IsFailure)
        {
            return petStatusOriginal.Error;
        }

        var pet = _pets.FirstOrDefault(p => p.Id == petId);

        if (pet is null)
        {
            return Errors.General.NotFound();
        }
        
        pet.UpdatePetStatus(petStatusOriginal.Value);

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

    public UnitResult<Error> AddPetPhotos(PetId petId, IReadOnlyList<PetPhoto> petPhotos)
    {
        if (Pets is null)
            return Errors.General.NotFound();

        var pet = Pets.FirstOrDefault(p => p.Id == petId);

        if (pet is null)
            return Errors.General.NotFound(petId.Value);

        var addPhotosRes = pet.AddPhotos(petPhotos);

        if (addPhotosRes.IsFailure)
            return addPhotosRes.Error;

        return UnitResult.Success<Error>();
    }
    
    public UnitResult<Error> AddPetPhoto(PetId petId, PetPhoto petPhoto)
    {
        if (Pets is null)
            return Errors.General.NotFound();

        var pet = Pets.FirstOrDefault(p => p.Id == petId);

        if (pet is null)
            return Errors.General.NotFound(petId.Value);

        var addPhotoRes = pet.AddPhoto(petPhoto);

        if (addPhotoRes.IsFailure)
            return addPhotoRes.Error;

        return UnitResult.Success<Error>();
    }
    
    public UnitResult<Error> RemovePetPhoto(PetId petId, PetPhoto petPhoto)
    {
        if (Pets is null)
            return Errors.General.NotFound();

        var pet = Pets.FirstOrDefault(p => p.Id == petId);

        if (pet is null)
            return Errors.General.NotFound(petId.Value);

        var addPhotosRes = pet.RemovePhoto(petPhoto);

        if (addPhotosRes.IsFailure)
            return addPhotosRes.Error;

        return UnitResult.Success<Error>();
    }
}