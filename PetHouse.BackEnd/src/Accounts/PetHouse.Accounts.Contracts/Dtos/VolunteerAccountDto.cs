
using PetHouse.Core.Dtos.PetManagment;

namespace PetHouse.Accounts.Contracts.Dtos;

public class VolunteerAccountDto
{
    public Guid Id { get; init; }
    
    public Guid UserId { get; init; }

    public IEnumerable<RequisiteDto> Requisites { get; init; } = [];
    public IEnumerable<CertificateDto> Certificates { get; init; } = [];
    public string Name { get; init; }
    public string Surname { get; init; }
}