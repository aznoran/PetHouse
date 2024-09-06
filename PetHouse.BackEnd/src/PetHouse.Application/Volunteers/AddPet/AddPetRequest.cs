using PetHouse.Application.Dto;
using PetHouse.Application.Dtos;
using PetHouse.Domain.Enums;

namespace PetHouse.Application.Volunteers.AddPet;

public record AddPetRequest(
    Guid VolunteerId,
    AddPetDto AddPetDto);
    
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
    string PhoneNumber,
    IEnumerable<RequisiteDto> RequisiteDtos,
    //TODO: посмотреть что с этим делать
    PetStatus PetStatus);