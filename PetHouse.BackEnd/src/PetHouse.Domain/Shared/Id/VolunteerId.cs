﻿namespace PetHouse.Domain.Models;

public record VolunteerId
{
    public VolunteerId(Guid value)
    {
        Value = value;
    }
    
    
    public Guid Value { get; }
    
    public static VolunteerId NewId => new(Guid.NewGuid());

    public static VolunteerId NewEmptyId => new(Guid.Empty);

    public static VolunteerId Create(Guid id) => new(id);
    
    public static implicit operator Guid(VolunteerId id)
    {
        return id.Value;
    }
}