﻿using PetHouse.Application.Abstraction;

namespace PetHouse.Application.PetManagement.Queries.GetAllPetWithPaginationAndFilter;

public record GetAllPetWithPaginationAndFilterQuery(
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
    int? PetStatus) : IQuery
{
    private GetAllPetWithPaginationAndFilterQuery(Guid id,
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
        int? PetStatus) 
        : this(Page,
        PageSize,
        SortBy,
        SortDirection,
        Name,
        SpeciesId,
        BreedId,
        Color,
        Weight,
        Height,
        IsCastrated,
        IsVaccinated,
        BirthdayDate,
        City,
        Street
        ,Country,
        PetStatus)
    {
        Id = id;
    }
    internal Guid Id { get; init; }

    public GetAllPetWithPaginationAndFilterQuery GetQueryWithId(Guid id, 
        GetAllPetWithPaginationAndFilterQuery query)
    {
        return new(id,
        query.Page,
        query.PageSize,
        query.SortBy,
        query.SortDirection,
        query.Name,
        query.SpeciesId,
        query.BreedId,
        query.Color,
        query.Weight,
        query.Height,
        query.IsCastrated,
        query.IsVaccinated,
        query.BirthdayDate,
        query.City,
        query.Street,
        query.Country,
        query.PetStatus);
    }
}