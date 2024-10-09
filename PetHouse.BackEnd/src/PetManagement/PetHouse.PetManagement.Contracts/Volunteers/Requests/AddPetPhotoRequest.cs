using Microsoft.AspNetCore.Http;

namespace PetHouse.PetManagement.Contracts.Volunteers.Requests;

public record AddPetPhotoRequest(
    IFormFile Photo,
    bool IsMain = false);