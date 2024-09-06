using CSharpFunctionalExtensions;
using PetHouse.Domain.Enums;
using PetHouse.Domain.Models.Volunteers.ValueObjects;
using PetHouse.Domain.Shared;
using PetHouse.Domain.ValueObjects;

namespace PetHouse.Domain.Models;

public sealed class Pet : Shared.Entity<PetId>, ISoftDeletable
{
    //EF CORE
    public Pet() { }
    public Pet(PetId petId,
        Name name,
        PetIdentifier petIdentifier,
        Description description,
        PetInfo petInfo, 
        Address address,
        PhoneNumber phoneNumber,
        RequisiteInfo requisites,
        PetStatus petStatus, 
        DateTime creationDate) : base(petId)
    {
        Name = name;
        PetIdentifier = petIdentifier;
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

    public PetIdentifier PetIdentifier { get; private set; }
    public Description Description { get; private set; }

    public PetInfo PetInfo { get; private set; }
    public Address Address { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }

    public RequisiteInfo Requisites { get; private set; }

    public PetPhotoInfo? PetPhotos { get; private set; }
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

    public void AddPhotos(PetPhotoInfo petPhotoInfo)
    {
        PetPhotos = petPhotoInfo;
    }
}