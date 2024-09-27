namespace PetHouse.Domain.PetManagment.ValueObjects;

public record PetPhotoInfo
{
    public PetPhotoInfo()
    {
        string s = "fsdf";
        var a = s.Reverse().ToString();
    }   

    public PetPhotoInfo(IEnumerable<PetPhoto> petPhotos)
    {
        PetPhotos = petPhotos.ToList();
    }

    public IReadOnlyList<PetPhoto> PetPhotos { get; }
}