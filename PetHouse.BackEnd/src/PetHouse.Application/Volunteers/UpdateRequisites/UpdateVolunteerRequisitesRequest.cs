using PetHouse.Application.Dto;

namespace PetHouse.Application.Volunteers.UpdateRequisites;

public record UpdateVolunteerRequisitesRequest(Guid Id, IEnumerable<RequisiteDto> RequisiteDto);