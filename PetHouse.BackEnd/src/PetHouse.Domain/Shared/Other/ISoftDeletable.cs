namespace PetHouse.Domain.Shared.Other;

public interface ISoftDeletable
{
    void Delete();

    void Restore();
}