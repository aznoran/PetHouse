﻿using Microsoft.AspNetCore.Mvc;
using PetHouse.API.Extensions;
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
        var res = await createVolunteerHandler.Handle(createVolunteerDto, cancellationToken);
        if (res.IsFailure)
        {
            return res.Error.ToResponse();
        }
        return Ok(res.Value);
    }
}
