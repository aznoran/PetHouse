using Microsoft.AspNetCore.Http;

namespace PetHouse.PetManagement.Contracts.Volunteers.Requests;

public record AddPetPhotosRequest(
    IFormFileCollection Photos,
    bool IsMain = false);