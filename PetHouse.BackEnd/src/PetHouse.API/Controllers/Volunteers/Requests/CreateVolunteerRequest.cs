using PetHouse.Application.Dtos.PetManagment;
using PetHouse.Application.Volunteers.Commands.Create;

namespace PetHouse.API.Controllers.Volunteers.Requests;

public record CreateVolunteerRequest(
    FullNameDto FullNameDto,
    string Description, 
    string Email,
    int YearsOfExperience, 
    string PhoneNumber,
    IEnumerable<SocialNetworksDto> SocialNetworksDto,
    IEnumerable<RequisiteDto> RequisiteDto)
{
    public CreateVolunteerCommand ToCommand()
    {
        return new CreateVolunteerCommand(FullNameDto, 
            Description,
            Email,
            YearsOfExperience, 
            PhoneNumber,
            SocialNetworksDto,
            RequisiteDto);
    }
}