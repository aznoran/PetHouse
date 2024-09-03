using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetHouse.API.Extensions;
using PetHouse.API.Response;
using PetHouse.Application.Dto;
using PetHouse.Application.Volunteers.Create;
using PetHouse.Application.Volunteers.UpdateMainInfo;
using PetHouse.Application.Volunteers.UpdateRequisites;
using PetHouse.Application.Volunteers.UpdateSocialNetworks;

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
    
    [HttpPut("{id:guid}/main-info")]
    public async Task<ActionResult<Guid>> UpdateMainInfo(
        [FromServices] IUpdateVolunteerMainInfoHandler updateVolunteerMainInfoHandler,
        [FromServices] IValidator<UpdateVolunteerMainInfoRequest> validator,
        [FromRoute] Guid id,
        [FromBody] UpdateVolunteerMainInfoDto updateVolunteerMainInfoDto,
        CancellationToken cancellationToken = default)
    {
        var request = new UpdateVolunteerMainInfoRequest(id, updateVolunteerMainInfoDto);
        
        var validateResult = await validator.ValidateAsync(request, cancellationToken);

        if (validateResult.IsValid == false)
            return validateResult.ToValidationErrorResponse();
        
        var res = await updateVolunteerMainInfoHandler.Handle(request, cancellationToken);

        if (res.IsFailure)
        {
            ResponseError temp = new ResponseError(res.Error.Code, res.Error.Message, null);
            return res.Error.ToResponse();
        }
        
        return new ObjectResult(res.Value) { StatusCode = 204 };
    }
    
    [HttpPatch("{id:guid}/requisites")]
    public async Task<ActionResult<Guid>> UpdateRequisites(
        [FromServices] IUpdateVolunteerRequisitesHandler updateVolunteerRequisitesHandler,
        [FromServices] IValidator<UpdateVolunteerRequisitesRequest> validator,
        [FromRoute] Guid id,
        [FromBody] UpdateVolunteerRequisitesDto updateVolunteerRequisitesDto,
        CancellationToken cancellationToken = default)
    {
        var request = new UpdateVolunteerRequisitesRequest(id, updateVolunteerRequisitesDto.RequisiteDto);

        var validateResult = await validator.ValidateAsync(request, cancellationToken);

        if (validateResult.IsValid == false)
            return validateResult.ToValidationErrorResponse();
        
        var res = await updateVolunteerRequisitesHandler.Handle(request, cancellationToken);

        if (res.IsFailure)
        {
            ResponseError temp = new ResponseError(res.Error.Code, res.Error.Message, null);
            return res.Error.ToResponse();
        }
        
        return new ObjectResult(res.Value) { StatusCode = 204 };
    }
    
    [HttpPatch("{id:guid}/social-networks")]
    public async Task<ActionResult<Guid>> UpdateSocialNetworks(
        [FromServices] IUpdateVolunteerSocialNetworksHandler updateVolunteerSocialNetworksHandler,
        [FromServices] IValidator<UpdateVolunteerSocialNetworksRequest> validator,
        [FromRoute] Guid id,
        [FromBody] UpdateVolunteerSocialNetworksDto updateVolunteerSocialNetworksDto,
        CancellationToken cancellationToken = default)
    {
        var request = new UpdateVolunteerSocialNetworksRequest(id, updateVolunteerSocialNetworksDto.SocialNetworksDtos);
            
        var validateResult = await validator.ValidateAsync(request, cancellationToken);

        if (validateResult.IsValid == false)
            return validateResult.ToValidationErrorResponse();
        
        var res = await updateVolunteerSocialNetworksHandler.Handle(request, cancellationToken);

        if (res.IsFailure)
        {
            ResponseError temp = new ResponseError(res.Error.Code, res.Error.Message, null);
            return res.Error.ToResponse();
        }
        
        return new ObjectResult(res.Value) { StatusCode = 204 };
    }
}