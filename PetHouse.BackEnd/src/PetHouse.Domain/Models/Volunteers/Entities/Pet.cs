using PetHouse.Domain.Enums;
using PetHouse.Domain.Shared;
using PetHouse.Domain.ValueObjects;

namespace PetHouse.Domain.Models;

public sealed class Pet : Entity<PetId>, ISoftDeletable
{
    private bool _isDeleted = false;
    public string Name { get; private set; }

    public PetIdentifier PetIdentifier { get; private set; }
    public string Description { get; private set; }

    public string Color { get; private set; }

    public string HealthInfo { get; private set; }

    public string Address { get; private set; }

    public double Weight { get; private set; }

    public double Height { get; private set; }

    public string PhoneNumber { get; private set; }

    public bool IsCastrated { get; private set; }

    public DateTime BirthdayDate { get; private set; }

    public bool IsVaccinated { get; private set; }

    public PetStatus PetStatus { get; private set; }

    public RequisiteInfo Requisites { get; private set; }

    public DateTime CreationDate { get; private set; }

    public PetPhotoInfo PetPhotos { get; private set; }
    public void Delete()
    {
        _isDeleted = true;
    }

    public void Restore()
    {
        _isDeleted = false;
    }
}