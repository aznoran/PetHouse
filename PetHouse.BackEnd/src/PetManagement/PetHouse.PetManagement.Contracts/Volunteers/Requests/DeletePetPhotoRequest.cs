namespace PetHouse.PetManagement.Contracts.Volunteers.Requests;

public record DeletePetPhotoRequest(
    string BucketName,
    string FileName);