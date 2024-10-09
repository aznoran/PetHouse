using Microsoft.EntityFrameworkCore;
using PetHouse.Core.Dtos.SpeciesManagment;

namespace PetHouse.SpecieManagement.Application;

public interface IReadDbContext
{
     DbSet<SpecieDto> Species { get; }
     DbSet<BreedDto> Breeds { get; }
}