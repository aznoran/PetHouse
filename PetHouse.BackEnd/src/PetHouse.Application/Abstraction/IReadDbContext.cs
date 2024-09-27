using Microsoft.EntityFrameworkCore;
using PetHouse.Application.Dtos.PetManagment;
using PetHouse.Application.Dtos.SpeciesManagment;

namespace PetHouse.Application.Volunteers;

public interface IReadDbContext
{
    DbSet<VolunteerDto> Volunteers { get; }
     DbSet<PetDto> Pets { get; }
     DbSet<SpecieDto> Species { get; }
     DbSet<BreedDto> Breeds { get; }
}