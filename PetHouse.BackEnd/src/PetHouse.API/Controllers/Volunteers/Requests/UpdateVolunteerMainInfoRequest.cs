using PetHouse.Application.Dto;
using PetHouse.Application.Volunteers.UpdateMainInfo;

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