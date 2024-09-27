using PetHouse.Application.Abstraction;
using PetHouse.Application.Dtos.PetManagment;

namespace PetHouse.Application.PetManagment.Commands.UpdateRequisites;

public record UpdateVolunteerRequisitesCommand(Guid Id, IEnumerable<RequisiteDto> RequisiteDto) : ICommand;