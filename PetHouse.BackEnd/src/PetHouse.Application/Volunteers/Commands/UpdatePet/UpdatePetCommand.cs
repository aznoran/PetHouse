using PetHouse.Application.Abstraction;
using PetHouse.Application.Dtos.PetManagment;
using PetHouse.Domain.PetManagment.Enums;

namespace PetHouse.Application.Volunteers.Commands.AddPet;

public record UpdatePetCommand(
    Guid VolunteerId,
    Guid PetId,
    EditPetDto EditPetDto) : ICommand;
    
public record EditPetDto(
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
    IEnumerable<RequisiteDto> RequisiteDtos);
    