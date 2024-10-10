using PetHouse.Core.Dtos.PetManagment;

namespace PetHouse.PetManagement.Contracts.Volunteers.Requests;

public record UpdatePetRequest(
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
    IEnumerable<RequisiteDto> RequisiteDtos
);