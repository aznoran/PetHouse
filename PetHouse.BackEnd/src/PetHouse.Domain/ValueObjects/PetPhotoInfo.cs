namespace PetHouse.Domain.ValueObjects;

public record PetPhotoInfo
{
    public ICollection<PetPhoto> PetPhotos { get; }
}