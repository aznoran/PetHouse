using CSharpFunctionalExtensions;
using PetHouse.Domain.PetManagement.Enums;
using PetHouse.Domain.Shared.Other;
using PetHouse.Domain.Shared.ValueObjects;

namespace PetHouse.Domain.PetManagement.ValueObjects;

public record PetStatusInfo
{
    private PetStatusInfo(PetStatus value)
    {
        Value = value;
    }
    
    public PetStatus Value { get; }
    
    public static Result<PetStatusInfo, Error> Create(int value)
    {
        PetStatus petStatusOriginal = (PetStatus)value;

        if (!Enum.IsDefined(typeof(PetStatus), petStatusOriginal))
        {
            return Errors.General.ValueIsInvalid("pet-status");
        }
        
        return new PetStatusInfo(petStatusOriginal);
    }
}