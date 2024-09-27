using CSharpFunctionalExtensions;
using PetHouse.Domain.PetManagment.Enums;
using PetHouse.Domain.PetManagment.ValueObjects;
using PetHouse.Domain.Shared.Id;
using PetHouse.Domain.Shared.Other;
using PetHouse.Domain.Shared.ValueObjects;

namespace PetHouse.Domain.PetManagment.Entities;

public sealed class Pet : Shared.ValueObjects.Entity<PetId>, ISoftDeletable
{
    //EF CORE
    public Pet()
    {
    }

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

    private bool _isDeleted = false;
    public Name Name { get; private set; }

    public Position Position { get; private set; }
    public PetIdentifier PetIdentifier { get; private set; }
    public Description Description { get; private set; }

    public PetInfo PetInfo { get; private set; }
    public Address Address { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }

    public IReadOnlyList<Requisite> Requisites { get; private set; }

    public IReadOnlyList<PetPhoto> PetPhotos { get; private set; }
    public PetStatus PetStatus { get; private set; }
    public DateTime CreationDate { get; private set; }

    public void Delete()
    {
        _isDeleted = true;
    }

    public void Restore()
    {
        _isDeleted = false;
    }

    public UnitResult<Error> AddPhotos(IReadOnlyList<PetPhoto> petPhotos)
    {
        if (petPhotos.Count(p => p.IsMain) > 1)
        {
            return Errors.General.ValueIsInvalid("isMain in petPhoto can't be more than 1");
        }
        
        PetPhotos = petPhotos;
        
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