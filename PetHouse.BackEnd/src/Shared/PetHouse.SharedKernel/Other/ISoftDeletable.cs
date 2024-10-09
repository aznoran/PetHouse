namespace PetHouse.SharedKernel.Other;

public interface ISoftDeletable
{
    void DeleteSoft();

    void Restore();
}