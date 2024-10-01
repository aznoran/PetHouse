using CSharpFunctionalExtensions;
using PetHouse.Domain.Shared.ValueObjects;

namespace PetHouse.Domain.PetManagement.ValueObjects;

public record PetPhoto
{
    public PetPhoto()
    {
        
    }
    private PetPhoto(FilePath path, bool isMain)
    {
        Path = path;
        IsMain = isMain;
    }

    public FilePath Path { get; }

    public bool IsMain { get; }

    public static Result<PetPhoto, Error> Create(FilePath path, bool isMain)
    {
        return new PetPhoto(path, isMain);
    }
}