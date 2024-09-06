using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetHouse.API.Extensions;
using PetHouse.Application.Dto;
using PetHouse.Application.Volunteers.AddPet;
using PetHouse.Application.Volunteers.Create;
using PetHouse.Application.Volunteers.Delete;
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
        [FromBody] CreateVolunteerRequest createVolunteerRequest,
        CancellationToken cancellationToken = default)
    {
        var res = await createVolunteerHandler.Handle(createVolunteerRequest, cancellationToken);

        if (res.IsFailure)
        {
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
            return res.Error.ToResponse();
        }
        
        return new ObjectResult(res.Value) { StatusCode = 204 };
    }
    
    [HttpPost("{id:guid}/pet")]
    public async Task<ActionResult<Guid>> AddPet(
        [FromServices] IAddPetHandler addPetHandler,
        [FromServices] IValidator<AddPetRequest> validator,
        [FromRoute] Guid id,
        [FromBody] AddPetDto addPetDto,
        CancellationToken cancellationToken = default)
    {
        var request = new AddPetRequest(id, addPetDto);
            
        var validateResult = await validator.ValidateAsync(request, cancellationToken);

        if (validateResult.IsValid == false)
            return validateResult.ToValidationErrorResponse();
        
        var res = await addPetHandler.Handle(request, cancellationToken);

        if (res.IsFailure)
        {
            return res.Error.ToResponse();
        }

        return Ok();
    }
}