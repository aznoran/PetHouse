using Microsoft.AspNetCore.Http;

namespace PetHouse.Application.Volunteers.AddPetPhoto;

public record AddPetPhotoRequest(Guid VolunteerId, Guid PetId, IEnumerable<IFormFile> Photos, bool IsMain = false);