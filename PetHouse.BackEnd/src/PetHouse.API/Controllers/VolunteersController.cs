using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetHouse.API.Extensions;
using PetHouse.API.Response;
using PetHouse.Application.Volunteers.Create;
using PetHouse.Application.Volunteers.Delete;
using SharpGrip.FluentValidation.AutoValidation.Shared.Extensions;

namespace PetHouse.API.Controllers;

[ApiController]
[Route("[controller]")]
public class VolunteersController : ApplicationController
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] ICreateVolunteerHandler createVolunteerHandler,
        [FromBody] CreateVolunteerRequest createVolunteerRequest,
        CancellationToken cancellationToken = default)
    {
        var res = await createVolunteerHandler.Handle(createVolunteerRequest, cancellationToken);

        if (res.IsFailure)
        {
            ResponseError temp = new ResponseError(res.Error.Code, res.Error.Message, null);
            return res.Error.ToResponse();
        }
        
        return new ObjectResult(res.Value) { StatusCode = 201 };
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Guid>> Delete(
        [FromServices] IDeleteVolunteerHandler deleteVolunteerHandler,
        [FromServices] IValidator<DeleteVolunteerRequest> validator,
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var request = new DeleteVolunteerRequest(id);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToValidationErrorResponse();
        }
        
        var res = await deleteVolunteerHandler.Handle(request, cancellationToken);

        if (res.IsFailure)
        {
            ResponseError temp = new ResponseError(res.Error.Code, res.Error.Message, null);
            return res.Error.ToResponse();
        }
        
        return new ObjectResult(res.Value) { StatusCode = 201 };
    }
}