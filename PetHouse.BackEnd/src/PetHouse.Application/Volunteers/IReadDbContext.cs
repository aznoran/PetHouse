using Microsoft.EntityFrameworkCore;
using PetHouse.Application.Dtos.PetManagment;
using PetHouse.Application.Dtos.SpeciesManagment;

namespace PetHouse.Application.Volunteers;

public interface IReadDbContext
{
    DbSet<VolunteerDto> Volunteers { get; }
    
    //DbSet<SpeciesDto> Species { get; }
}