using PetHouse.Domain.Models.Shared.ValueObjects;

namespace PetHouse.Application.Providers;

public record FileData(Stream Content, FilePath FilePath, string BucketName);