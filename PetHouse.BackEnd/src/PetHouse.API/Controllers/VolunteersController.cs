using Microsoft.AspNetCore.Mvc;
using PetHouse.Application.Volunteers.CreateVolunteer;

namespace PetHouse.API.Controllers;

[ApiController]
[Route("[controller]")]
public class VolunteersController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromServices] CreateVolunteerHandler createVolunteerHandler,
        [FromBody] CreateVolunteerRequest createVolunteerDto,
        CancellationToken cancellationToken = default)
    {
        Guid res = await createVolunteerHandler.Handle(createVolunteerDto, cancellationToken);
        return Ok(res);
    }
}
