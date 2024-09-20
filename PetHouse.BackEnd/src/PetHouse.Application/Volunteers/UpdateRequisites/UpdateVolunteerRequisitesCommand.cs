using PetHouse.Application.Dto;

namespace PetHouse.Application.Volunteers.UpdateRequisites;

public record UpdateVolunteerRequisitesCommand(Guid Id, IEnumerable<RequisiteDto> RequisiteDto);