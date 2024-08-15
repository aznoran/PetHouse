﻿namespace PetHouse.Domain;

public record PetId 
{
    public PetId(Guid value)
    {
        Value = value;
    }
    
    public Guid Value { get; }
    
    public static PetId NewId => new(Guid.NewGuid());

    public static PetId NewEmptyId => new(Guid.Empty);

    public static PetId Create(Guid id) => new(id);
}