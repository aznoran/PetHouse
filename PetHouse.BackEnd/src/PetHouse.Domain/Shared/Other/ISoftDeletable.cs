namespace PetHouse.Domain.Shared.Other;

public interface ISoftDeletable
{
    void DeleteSoft();

    void Restore();
}