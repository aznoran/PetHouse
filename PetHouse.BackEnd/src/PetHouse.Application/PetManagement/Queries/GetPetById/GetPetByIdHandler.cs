﻿using Microsoft.EntityFrameworkCore;
using PetHouse.Application.Abstraction;
using PetHouse.Application.Dtos.PetManagment;

namespace PetHouse.Application.PetManagement.Queries.GetPetById;

public class GetPetByIdHandler : IQueryHandler<GetPetByIdQuery, PetDto?>
{
    private readonly IReadDbContext _readDbContext;

    public GetPetByIdHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<PetDto?> Handle(GetPetByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var pet = await _readDbContext.Pets
            .Where(p => p.VolunteerId == query.VolunteerId && p.Id == query.PetId)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (pet is { PetPhotos: not null })
        {
            pet.PetPhotos = pet.PetPhotos.OrderByDescending(photo => photo.IsPhotoMain).ToList();
        }

        return pet;
    }
}