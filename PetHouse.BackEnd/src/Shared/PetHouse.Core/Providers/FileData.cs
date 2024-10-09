using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.Core.Providers;

public record FileData(Stream Content, FileInfo FileInfo);

public record FileInfo(FilePath Path, string BucketName);