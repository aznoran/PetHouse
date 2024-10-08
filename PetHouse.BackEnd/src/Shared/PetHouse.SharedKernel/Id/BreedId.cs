﻿namespace PetHouse.SharedKernel.Id;

public record BreedId
{
    private BreedId(Guid value)
    {
        Value = value;
    }
    
    
    public Guid Value { get; }
    
    public static BreedId NewId => new(Guid.NewGuid());

    public static BreedId NewEmptyId => new(Guid.Empty);

    public static BreedId Create(Guid id) => new(id);
}