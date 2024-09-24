using PetHouse.Domain.Models.Shared.ValueObjects;

namespace PetHouse.Application.Providers;

public record FileData(Stream Content, FileInfo FileInfo);

public record FileInfo(FilePath Path, string BucketName);