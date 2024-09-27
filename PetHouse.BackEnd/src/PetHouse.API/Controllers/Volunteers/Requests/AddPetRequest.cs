using PetHouse.Application.Dtos.PetManagment;
using PetHouse.Application.PetManagment.Commands.AddPet;
using PetHouse.Domain.PetManagment.Enums;

namespace PetHouse.API.Controllers.Volunteers.Requests;

public record AddPetRequest(
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
    int PetStatus
)
{
    public AddPetCommand ToCommand(Guid id)
    {
        if (Enum.TryParse(PetStatus.ToString(), out PetStatus petStatus) == false)
        {
            throw new ArgumentException("Invalid parameter for PetStatus ENUM");
        }

        return new AddPetCommand(id, new AddPetDto(
            Name,
            PetIdentifierDto,
            Description,
            Color,
            HealthInfo,
            Weight,
            Height,
            IsCastrated,
            IsVaccinated,
            BirthdayDate,
            City,
            Street,
            Country,
            PhoneNumber,
            RequisiteDtos,
            petStatus));
    }
}