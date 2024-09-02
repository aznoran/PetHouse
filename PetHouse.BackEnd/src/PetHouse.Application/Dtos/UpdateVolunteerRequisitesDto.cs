using PetHouse.Application.Dto;

namespace PetHouse.Application.Volunteers.UpdateRequisites;

public record UpdateVolunteerRequisitesDto(IEnumerable<RequisiteDto> RequisiteDto);