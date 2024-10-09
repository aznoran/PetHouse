using Microsoft.EntityFrameworkCore;
using PetHouse.Core.Dtos.PetManagment;

namespace PetHouse.PetManagement.Application;

public interface IReadDbContext
{
     DbSet<VolunteerDto> Volunteers { get; }
     DbSet<PetDto> Pets { get; }
}