using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using PetHouse.Domain.Shared;

namespace PetHouse.Domain.Models;

public class PetPhoto
{
    public PetPhoto()
    {
        
    }
    private PetPhoto(string path, bool isMain)
    {
        Path = path;
        IsMain = isMain;
    }

    public string Path { get; }

    public bool IsMain { get; }

    public static Result<PetPhoto, Error> Create(string path, bool isMain)
    {
        return new PetPhoto(path, isMain);
    }
}