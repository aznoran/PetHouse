using PetHouse.Application.Dto;
using PetHouse.Application.Volunteers.UpdateMainInfo;

namespace PetHouse.Application.Volunteers.Create;

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