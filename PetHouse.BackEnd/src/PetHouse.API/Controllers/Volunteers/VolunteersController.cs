using Microsoft.AspNetCore.Mvc;
using PetHouse.API.Controllers.Shared;
using PetHouse.API.Controllers.Volunteers.Requests;
using PetHouse.API.Extensions;
using PetHouse.API.Processors;
using PetHouse.Application.Dto;
using PetHouse.Application.Volunteers.AddPet;
using PetHouse.Application.Volunteers.AddPetPhoto;
using PetHouse.Application.Volunteers.Create;
using PetHouse.Application.Volunteers.Delete;
using PetHouse.Application.Volunteers.UpdateMainInfo;
using PetHouse.Application.Volunteers.UpdateRequisites;
using PetHouse.Application.Volunteers.UpdateSocialNetworks;

namespace PetHouse.API.Controllers.Volunteers;

[ApiController]
[Route("[controller]")]
public class VolunteersController : ApplicationController
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] CreateVolunteerHandler createVolunteerHandler,
        [FromBody] CreateVolunteerRequest createVolunteerRequest,
        CancellationToken cancellationToken = default)
    {
        var res = await createVolunteerHandler.Handle(createVolunteerRequest.ToCommand(), cancellationToken);

        if (res.IsFailure)
        {
            return res.Error.ToResponse();
        }

        return new ObjectResult(res.Value) { StatusCode = 201 };
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Guid>> Delete(
        [FromServices] DeleteVolunteerHandler deleteVolunteerHandler,
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteVolunteerCommand(id);

        var res = await deleteVolunteerHandler.Handle(command, cancellationToken);

        if (res.IsFailure)
        {
            return res.Error.ToResponse();
        }

        return new ObjectResult(res.Value) { StatusCode = 201 };
    }

    [HttpPut("{id:guid}/main-info")]
    public async Task<ActionResult<Guid>> UpdateMainInfo(
        [FromServices] UpdateVolunteerMainInfoHandler updateVolunteerMainInfoHandler,
        [FromRoute] Guid id,
        [FromBody] UpdateVolunteerMainInfoRequest updateVolunteerMainInfoRequest,
        CancellationToken cancellationToken = default)
    {
        var res = await updateVolunteerMainInfoHandler.Handle(updateVolunteerMainInfoRequest.ToCommand(id), cancellationToken);

        if (res.IsFailure)
        {
            return res.Error.ToResponse();
        }

        return new ObjectResult(res.Value) { StatusCode = 204 };
    }

    [HttpPatch("{id:guid}/requisites")]
    public async Task<ActionResult<Guid>> UpdateRequisites(
        [FromServices] UpdateVolunteerRequisitesHandler updateVolunteerRequisitesHandler,
        [FromRoute] Guid id,
        [FromBody] IEnumerable<RequisiteDto> updateVolunteerRequisitesDto,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateVolunteerRequisitesCommand(id, updateVolunteerRequisitesDto);

        var res = await updateVolunteerRequisitesHandler.Handle(command, cancellationToken);

        if (res.IsFailure)
        {
            return res.Error.ToResponse();
        }

        return new ObjectResult(res.Value) { StatusCode = 204 };
    }

    [HttpPatch("{id:guid}/social-networks")]
    public async Task<ActionResult<Guid>> UpdateSocialNetworks(
        [FromServices] UpdateVolunteerSocialNetworksHandler updateVolunteerSocialNetworksHandler,
        [FromRoute] Guid id,
        [FromBody] IEnumerable<SocialNetworksDto> updateVolunteerSocialNetworksDto,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateVolunteerSocialNetworksCommand(id, updateVolunteerSocialNetworksDto);

        var res = await updateVolunteerSocialNetworksHandler.Handle(command, cancellationToken);

        if (res.IsFailure)
        {
            return res.Error.ToResponse();
        }

        return new ObjectResult(res.Value) { StatusCode = 204 };
    }

    [HttpPost("{id:guid}/pet")]
    public async Task<ActionResult<Guid>> AddPet(
        [FromServices] AddPetHandler addPetHandler,
        [FromRoute] Guid id,
        [FromBody] AddPetRequest addPetRequest,
        CancellationToken cancellationToken = default)
    {
        var res = await addPetHandler.Handle(addPetRequest.ToCommand(id), cancellationToken);

        if (res.IsFailure)
        {
            return res.Error.ToResponse();
        }

        return Ok();
    }

    [HttpPost("{volunteerId:guid}/petphotos/{petId:guid}")]
    public async Task<ActionResult<Guid>> AddPetPhotos(
        [FromServices] AddPetPhotosHandler addPetPhotosHandler,
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromForm] AddPetPhotoRequest request,
        CancellationToken cancellationToken = default)
    {
        await using var fileProcessor = new FormFileProcessor();
        
        IEnumerable<UploadFileDto> uploadFilesDto = fileProcessor.Process(request.Photos);
        
        var res = await addPetPhotosHandler.Handle(request.ToCommand(volunteerId, petId, uploadFilesDto), cancellationToken);

        if (res.IsFailure)
        {
            return res.Error.ToResponse();
        }

        return new ObjectResult(res.Value) { StatusCode = 201 };
    }
}