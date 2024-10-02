using System.ComponentModel.DataAnnotations;
using PetHouse.Application.Abstraction;

namespace PetHouse.Application.PetManagement.Queries.GetAllPetsWithPaginationAndFilter;

public record GetAllPetsWithPaginationAndFilterQuery(
    int Page,
    int PageSize,
    string? SortBy,
    string? SortDirection,
    string? Name,
    Guid? SpeciesId,
    Guid? BreedId,
    string? Color,
    double? Weight,
    double? Height,
    bool? IsCastrated,
    bool? IsVaccinated,
    DateTime? BirthdayDate,
    string? City,
    string? Street,
    string? Country,
    int? PetStatus) : IQuery;
