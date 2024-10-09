using CSharpFunctionalExtensions;
using PetHouse.PetManagement.Domain.Aggregate;
using PetHouse.PetManagement.Domain.Enums;
using PetHouse.PetManagement.Domain.ValueObjects;
using PetHouse.SharedKernel.Id;
using PetHouse.SharedKernel.Other;
using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.PetManagement.Domain.Entities;

public sealed class Pet : SharedKernel.ValueObjects.Entity<PetId>, ISoftDeletable
{
    private bool _isDeleted = false;
    
    //EF CORE NAVIGATION PROPERTY
    public Volunteer Volunteer { get; init; }
    
    //EF CORE
    // ReSharper disable once UnusedMember.Local
    private Pet(PetId id) : base(id) { }

    public Pet(PetId petId,
        Name name,
        PetIdentifier petIdentifier,
        Position position,
        Description description,
        PetInfo petInfo,
        Address address,
        PhoneNumber phoneNumber,
        IReadOnlyList<Requisite> requisites,
        PetStatus petStatus,
        DateTime creationDate) : base(petId)
    {
        Name = name;
        PetIdentifier = petIdentifier;
        Position = position;
        Description = description;
        PetInfo = petInfo;
        Address = address;
        PhoneNumber = phoneNumber;
        Requisites = requisites;
        PetStatus = petStatus;
        CreationDate = creationDate;
    }
    public Name Name { get; private set; }

    public Position Position { get; private set; }
    public PetIdentifier PetIdentifier { get; private set; }
    public Description Description { get; private set; }

    public PetInfo PetInfo { get; private set; }
    public Address Address { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }

    public IReadOnlyList<Requisite> Requisites { get; private set; }
    
    private List<PetPhoto> _petPhotos = [];
    public IReadOnlyList<PetPhoto>? PetPhotos => _petPhotos;
    
    public PetStatus PetStatus { get; private set; }
    public DateTime CreationDate { get; private set; }

    public void DeleteSoft()
    {
        _isDeleted = true;
    }

    public void Restore()
    {
        _isDeleted = false;
    }

    public void UpdatePetStatus(PetStatus petStatus)
    {
        PetStatus = petStatus;
    }
    
    public UnitResult<Error> Update(Name name,
        PetIdentifier petIdentifier,
        Description description,
        PetInfo petInfo,
        Address address,
        PhoneNumber phoneNumber,
        IReadOnlyList<Requisite> requisites,
        DateTime creationDate)
    {
        Name = name;
        PetIdentifier = petIdentifier;
        Description = description;
        PetInfo = petInfo;
        Address = address;
        PhoneNumber = phoneNumber;
        Requisites = requisites;
        CreationDate = creationDate;

        return UnitResult.Success<Error>();
    }
    
    public UnitResult<Error> AddPhotos(IReadOnlyList<PetPhoto> petPhotos)
    {
        if (petPhotos.Count(p => p.IsMain) > 1)
        {
            return Errors.General.ValueIsInvalid("isMain in petPhoto can't be more than 1");
        }
        
        _petPhotos = petPhotos.ToList();
        
        return UnitResult.Success<Error>();
    }
    
    public UnitResult<Error> ChangeMainPhoto(FilePath filePath)
    {
        var newPetPhotos = new List<PetPhoto>();
        
        foreach (var petPhoto in _petPhotos)
        {
            PetPhoto petPhotoTemp;
            if (petPhoto.Path == filePath)
            {
                petPhotoTemp = PetPhoto.Create(petPhoto.Path, true).Value;
                newPetPhotos.Add(petPhotoTemp);
            }
            else
            {
                petPhotoTemp = PetPhoto.Create(petPhoto.Path, false).Value;
                newPetPhotos.Add(petPhotoTemp);
            }
        }

        _petPhotos = newPetPhotos.ToList();
        
        return UnitResult.Success<Error>();
    }
    public UnitResult<Error> AddPhoto(PetPhoto petPhoto)
    {
        List<PetPhoto> newPetPhotos = new List<PetPhoto>();
        
        if (_petPhotos is null)
        {
            newPetPhotos.Add(petPhoto);
        }
        else
        {
            if (((petPhoto.IsMain ? 1 : 0) + _petPhotos.Count(p => p.IsMain)) > 1)
            {
                return Errors.General.ValueIsInvalid("isMain in petPhoto can't be more than 1");
            }

            foreach (var petPhotoTemp in _petPhotos)
            {
                newPetPhotos.Add(PetPhoto.Create(petPhotoTemp.Path, petPhotoTemp.IsMain).Value);
            }
            
            newPetPhotos.Add(petPhoto);
        }

        _petPhotos = newPetPhotos.ToList();
        
        return UnitResult.Success<Error>();
    }
    
    public UnitResult<Error> RemovePhoto(PetPhoto petPhoto)
    {
        if (_petPhotos is null)
        {
            return Errors.General.NotFound();
        }
        
        List<PetPhoto> newPetPhotos = new List<PetPhoto>();

        foreach (var petPhotoTemp in _petPhotos)
        {
            if (petPhotoTemp.Path != petPhoto.Path)
            {
                newPetPhotos.Add(petPhotoTemp);
            }
        }
        
        _petPhotos = newPetPhotos.ToList();
        
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> Move(Position newPosition)
    {
        this.Position = newPosition;
        
        return UnitResult.Success<Error>();
    }
    
    public UnitResult<Error> MoveForward()
    {
        var position = Position.Create(this.Position.Value + 1);

        if (position.IsFailure) return position.Error;
        
        this.Position = position.Value;

        return UnitResult.Success<Error>();
    }
    
    public UnitResult<Error> MoveBack()
    {
        var position = Position.Create(this.Position.Value - 1);

        if (position.IsFailure) return position.Error;
        
        this.Position = position.Value;
        
        return UnitResult.Success<Error>();
    }
}