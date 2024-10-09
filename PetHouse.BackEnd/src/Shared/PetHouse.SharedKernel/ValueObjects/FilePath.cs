using CSharpFunctionalExtensions;
using PetHouse.SharedKernel.Other;

namespace PetHouse.SharedKernel.ValueObjects;

public record FilePath
{
    private FilePath(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<FilePath, Error> Create(Guid value, string extension)
    {
        // валидация на доступные расширения файлов

        var fullPath = value + extension;

        return new FilePath(fullPath);
    }

    public static Result<FilePath, Error> Create(string fullPath)
    {
        if (string.IsNullOrWhiteSpace(fullPath))
            return Errors.General.ValueIsInvalid("file path");

        return new FilePath(fullPath);
    }
}