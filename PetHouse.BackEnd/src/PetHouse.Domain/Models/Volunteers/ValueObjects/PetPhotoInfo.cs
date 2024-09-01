using PetHouse.Domain.Models;

namespace PetHouse.Domain.ValueObjects;

public record PetPhotoInfo
{
    public PetPhotoInfo()
    {
    }

    public PetPhotoInfo(IEnumerable<PetPhoto> petPhotos)
    {
        PetPhotos = petPhotos.ToList();
    }

    public IReadOnlyList<PetPhoto> PetPhotos { get; }
}