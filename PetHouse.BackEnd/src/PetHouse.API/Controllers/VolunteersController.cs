using Microsoft.AspNetCore.Mvc;
using PetHouse.API.Extensions;
using PetHouse.API.Response;
using PetHouse.Application.Volunteers.CreateVolunteer;

namespace PetHouse.API.Controllers;

[ApiController]
[Route("[controller]")]
public class VolunteersController : ApplicationController
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] ICreateVolunteerHandler createVolunteerHandler,
        [FromBody] CreateVolunteerRequest createVolunteerDto,
        CancellationToken cancellationToken = default)
    {
        var res = await createVolunteerHandler.Handle(createVolunteerDto, cancellationToken);

        if (res.IsFailure)
        {
            ResponseError temp = new ResponseError(res.Error.Code, res.Error.Message, null);
            return res.Error.ToResponse();
        }
        
        return new ObjectResult(res.Value) { StatusCode = 201 };
    }
}