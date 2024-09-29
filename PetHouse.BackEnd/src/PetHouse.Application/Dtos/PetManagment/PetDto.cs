using PetHouse.Application.PetManagement.Commands.AddPet;
using PetHouse.Domain.PetManagment.Enums;
using PetHouse.Domain.PetManagment.ValueObjects;

namespace PetHouse.Application.Dtos.PetManagment;

public class PetDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public int Position { get; init; }
    public PetIdentifierDto PetIdentifier { get; init; }
    public string Description { get; init; }
    public string Color { get; init; }
    public string HealthInfo { get; init; }
    public double Weight { get; init; }
    public double Height { get; init; }
    public bool IsCastrated { get; init; }
    public bool IsVaccinated { get; init; }
    public DateTime BirthdayDate { get; init; }
    public string City { get; init; }
    public string Street { get; init; }
    public string Country { get; init; }
    public string PhoneNumber { get; init; }
    public IEnumerable<RequisiteDto> Requisites { get; init; }
    public IEnumerable<PetPhotoDto>? PetPhotos { get; init; }
    public PetStatus PetStatus { get; init; }
}