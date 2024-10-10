using PetHouse.Core.Abstraction;
using PetHouse.Core.Dtos.PetManagment;

namespace PetHouse.PetManagement.Application.Commands.UpdateRequisites;

public record UpdateVolunteerRequisitesCommand(Guid Id, IEnumerable<RequisiteDto> RequisiteDto) : ICommand;