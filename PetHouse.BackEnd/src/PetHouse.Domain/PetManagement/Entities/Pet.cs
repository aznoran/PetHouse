﻿using CSharpFunctionalExtensions;
using PetHouse.Domain.PetManagement.Aggregate;
using PetHouse.Domain.PetManagement.ValueObjects;
using PetHouse.Domain.Shared.Id;
using PetHouse.Domain.Shared.Other;
using PetHouse.Domain.Shared.ValueObjects;

namespace PetHouse.Domain.PetManagement.Entities;

public sealed class Pet : Shared.ValueObjects.Entity<PetId>, ISoftDeletable
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
        PetStatusInfo petStatus,
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
    
    public PetStatusInfo PetStatus { get; private set; }
    public DateTime CreationDate { get; private set; }

    public void DeleteSoft()
    {
        _isDeleted = true;
    }

    public void Restore()
    {
        _isDeleted = false;
    }

    public void UpdatePetStatus(PetStatusInfo petStatus)
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
        var enumerable = petPhotos.ToList();
        
        if (enumerable.Count(p => p.IsMain) > 1)
        {
            return Errors.General.ValueIsInvalid("isMain in petPhoto can't be more than 1");
        }
        
        _petPhotos = enumerable;
        
        return UnitResult.Success<Error>();
    }
    
    public UnitResult<Error> AddPhoto(PetPhoto petPhoto)
    {
        if (_petPhotos is null)
        {
            _petPhotos = new List<PetPhoto>() { petPhoto };
        }
        else
        {
            if (((petPhoto.IsMain ? 1 : 0) + _petPhotos.Count(p => p.IsMain)) > 1)
            {
                return Errors.General.ValueIsInvalid("isMain in petPhoto can't be more than 1");
            }
            
            _petPhotos.Add(petPhoto);
        }
        
        return UnitResult.Success<Error>();
    }
    
    public UnitResult<Error> RemovePhoto(PetPhoto petPhoto)
    {
        if (_petPhotos is null)
        {
            return Errors.General.NotFound();
        }
        
        var removeRes = _petPhotos.Remove(petPhoto);

        if (removeRes == false)
        {
            return Errors.General.NotFound();
        }
        
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