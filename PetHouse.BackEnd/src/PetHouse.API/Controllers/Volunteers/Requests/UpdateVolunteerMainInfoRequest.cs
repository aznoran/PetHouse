using PetHouse.Application.Dtos.PetManagment;
using PetHouse.Application.Volunteers.Commands.UpdateMainInfo;

namespace PetHouse.API.Controllers.Volunteers.Requests;

public record UpdateVolunteerMainInfoRequest(
    FullNameDto FullNameDto,
    string Email,
    string Description,
    int YearsOfExperience,
    string PhoneNumber
)
{
    public UpdateVolunteerMainInfoCommand ToCommand(Guid id)
    {
        return new UpdateVolunteerMainInfoCommand(id,
            new UpdateVolunteerMainInfoDto(FullNameDto,
                Email,
                Description,
                YearsOfExperience, 
                PhoneNumber));
    }
}