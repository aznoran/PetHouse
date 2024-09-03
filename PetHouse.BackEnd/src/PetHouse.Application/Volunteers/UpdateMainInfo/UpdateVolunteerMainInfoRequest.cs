namespace PetHouse.Application.Volunteers.UpdateMainInfo;

public record UpdateVolunteerMainInfoRequest(
    Guid Id,
    UpdateVolunteerMainInfoDto UpdateVolunteerMainInfoDto
);