using PetHouse.Application.Abstraction;
using PetHouse.Application.Dtos.PetManagment;
using PetHouse.Domain.PetManagment.Enums;

namespace PetHouse.Application.PetManagement.Commands.AddPet;

public record AddPetCommand(
    Guid VolunteerId,
    AddPetDto AddPetDto) : ICommand;
    
public record AddPetDto(
    string Name,
    PetIdentifierDto PetIdentifierDto,
    string Description,
    string Color,
    string HealthInfo,
    double Weight,
    double Height,
    bool IsCastrated,
    bool IsVaccinated,
    DateTime BirthdayDate,
    string City,
    string Street,
    string Country,
    string PhoneNumber,
    IEnumerable<RequisiteDto> RequisiteDtos,
    PetStatus PetStatus);
    
public record PetIdentifierDto(
    Guid SpeciesId,
    Guid BreedId);