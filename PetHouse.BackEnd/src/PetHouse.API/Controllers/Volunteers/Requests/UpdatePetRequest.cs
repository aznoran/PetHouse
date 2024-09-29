using PetHouse.Application.Dtos.PetManagment;
using PetHouse.Application.PetManagement.Commands.AddPet;
using PetHouse.Application.PetManagement.Commands.UpdatePet;

namespace PetHouse.API.Controllers.Volunteers.Requests;

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
)
{
    public UpdatePetCommand ToCommand(Guid volunteerId, Guid petId)
    {
        return new UpdatePetCommand(
            volunteerId,
            petId,
            new EditPetDto(Name,
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
                RequisiteDtos));
    }
}