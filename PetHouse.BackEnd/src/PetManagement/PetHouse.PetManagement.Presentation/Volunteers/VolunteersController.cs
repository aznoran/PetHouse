using Microsoft.AspNetCore.Mvc;
using PetHouse.Core.Dtos.PetManagment;
using PetHouse.Core.Dtos.Shared;
using PetHouse.Core.Extensions;
using PetHouse.Framework;
using PetHouse.Framework.Authorization;
using PetHouse.PetManagement.Application.Commands.AddPet;
using PetHouse.PetManagement.Application.Commands.AddPetPhoto;
using PetHouse.PetManagement.Application.Commands.AddPetPhotos;
using PetHouse.PetManagement.Application.Commands.ChangePetMainPhoto;
using PetHouse.PetManagement.Application.Commands.Create;
using PetHouse.PetManagement.Application.Commands.Delete;
using PetHouse.PetManagement.Application.Commands.DeletePet;
using PetHouse.PetManagement.Application.Commands.DeletePetPhoto;
using PetHouse.PetManagement.Application.Commands.DeletePetSoft;
using PetHouse.PetManagement.Application.Commands.UpdateMainInfo;
using PetHouse.PetManagement.Application.Commands.UpdatePet;
using PetHouse.PetManagement.Application.Commands.UpdatePetStatus;
using PetHouse.PetManagement.Application.Commands.UpdateRequisites;
using PetHouse.PetManagement.Application.Commands.UpdateSocialNetworks;
using PetHouse.PetManagement.Application.Queries.GetAllWithPagination;
using PetHouse.PetManagement.Application.Queries.GetVolunteerById;
using PetHouse.PetManagement.Contracts.Volunteers.Requests;
using PetHouse.PetManagement.Presentation.Processors;
using PetHouse.SharedKernel.Constraints;

namespace PetHouse.PetManagement.Presentation.Volunteers;

public class VolunteersController : ApplicationController
{
    [Permission(Policies.PetManagement.Get)]
    [HttpGet]
    public async Task<ActionResult> GetAll(
        [FromServices] GetAllVolunteerWithPaginationHandler getAllVolunteerWithPaginationHandler,
        [FromQuery] GetAllVolunteerWithPaginationQuery query,
        CancellationToken cancellationToken = default)
    {
        var res = await getAllVolunteerWithPaginationHandler.Handle(query, cancellationToken);

        return new ObjectResult(res) { StatusCode = 201 };
    }

    [Permission(Policies.PetManagement.Get)]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Guid>> GetVolunteer(
        [FromServices] GetVolunteerByIdHandler getAllWithPaginationHandler,
        [FromRoute] Guid id,
        [FromQuery] GetVolunteerByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var res = await getAllWithPaginationHandler.Handle(query.GetQueryWithId(id), cancellationToken);

        return new ObjectResult(res) { StatusCode = 201 };
    }

    [Permission(Policies.PetManagement.Create)]
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] CreateVolunteerHandler createVolunteerHandler,
        [FromBody] CreateVolunteerRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateVolunteerCommand(
            request.FullNameDto,
            request.Description,
            request.Email,
            request.YearsOfExperience,
            request.PhoneNumber,
            request.SocialNetworksDto,
            request.RequisiteDto);

        var res = await createVolunteerHandler.Handle(command, cancellationToken);

        if (res.IsFailure)
        {
            return res.Error.ToResponse();
        }

        return new ObjectResult(res.Value) { StatusCode = 201 };
    }

    [Permission(Policies.PetManagement.UpdatePet)]
    [HttpPost("{volunteerId:guid}/petphoto/{petId:guid}")]
    public async Task<ActionResult<Guid>> AddPetPhoto(
        [FromServices] AddPetPhotoHandler addPetPhotoHandler,
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromForm] AddPetPhotoRequest request,
        CancellationToken cancellationToken = default)
    {
        await using var file = request.Photo.OpenReadStream();

        var uploadFileDto = new UploadFileDto(file, request.Photo.FileName);

        var command = new AddPetPhotoCommand(
            volunteerId,
            petId,
            uploadFileDto,
            request.IsMain);

        var res = await addPetPhotoHandler
            .Handle(command, cancellationToken);

        if (res.IsFailure)
        {
            return res.Error.ToResponse();
        }

        return new ObjectResult(res.Value) { StatusCode = 201 };
    }

    [Permission(Policies.PetManagement.CreatePet)]
    [HttpPost("{id:guid}/pet")]
    public async Task<ActionResult> AddPet(
        [FromServices] AddPetHandler addPetHandler,
        [FromRoute] Guid id,
        [FromBody] AddPetRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new AddPetCommand(
            id,
            new AddPetDto(
                request.Name,
                request.PetIdentifierDto,
                request.Description,
                request.Color,
                request.HealthInfo,
                request.Weight,
                request.Height,
                request.IsCastrated,
                request.IsVaccinated,
                request.BirthdayDate,
                request.City,
                request.Street,
                request.Country,
                request.PhoneNumber,
                request.RequisiteDtos,
                request.PetStatus));

        var res = await addPetHandler.Handle(command, cancellationToken);

        if (res.IsFailure)
        {
            return res.Error.ToResponse();
        }

        return Ok();
    }

    [Permission(Policies.PetManagement.UpdatePet)]
    [HttpPost("{volunteerId:guid}/petphotos/{petId:guid}")]
    public async Task<ActionResult<Guid>> AddPetPhotos(
        [FromServices] AddPetPhotosHandler addPetPhotosHandler,
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromForm] AddPetPhotosRequest request,
        CancellationToken cancellationToken = default)
    {
        await using var fileProcessor = new FormFileProcessor();

        IEnumerable<UploadFileDto> uploadFilesDto = fileProcessor.Process(request.Photos);

        var command = new AddPetPhotosCommand(
            volunteerId,
            petId,
            uploadFilesDto,
            request.IsMain);

        var res = await addPetPhotosHandler.Handle(command, cancellationToken);

        if (res.IsFailure)
        {
            return res.Error.ToResponse();
        }

        return new ObjectResult(res.Value) { StatusCode = 201 };
    }

    [Permission(Policies.PetManagement.Update)]
    [HttpPut("{id:guid}/main-info")]
    public async Task<ActionResult<Guid>> UpdateMainInfo(
        [FromServices] UpdateVolunteerMainInfoHandler updateVolunteerMainInfoHandler,
        [FromRoute] Guid id,
        [FromBody] UpdateVolunteerMainInfoRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateVolunteerMainInfoCommand(
            id, new UpdateVolunteerMainInfoDto(
                request.FullNameDto,
                request.Email,
                request.Description,
                request.YearsOfExperience,
                request.PhoneNumber));

        var res = await updateVolunteerMainInfoHandler.Handle(command, cancellationToken);

        if (res.IsFailure)
        {
            return res.Error.ToResponse();
        }

        return new ObjectResult(res.Value) { StatusCode = 204 };
    }

    [Permission(Policies.PetManagement.UpdatePet)]
    [HttpPut("{volunteerId:guid}/pet/{petId:guid}")]
    public async Task<ActionResult> UpdatePetMainInfo(
        [FromServices] UpdatePetHandler updatePetHandler,
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromBody] UpdatePetRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdatePetCommand(
            volunteerId,
            petId,
            new EditPetDto(
                request.Name,
                request.PetIdentifierDto,
                request.Description,
                request.Color,
                request.HealthInfo,
                request.Weight,
                request.Height,
                request.IsCastrated,
                request.IsVaccinated,
                request.BirthdayDate,
                request.City,
                request.Street,
                request.Country,
                request.PhoneNumber,
                request.RequisiteDtos));

        var res = await updatePetHandler
            .Handle(command, cancellationToken);

        if (res.IsFailure)
        {
            return res.Error.ToResponse();
        }

        return new ObjectResult(res.IsSuccess) { StatusCode = 204 };
    }

    [Permission(Policies.PetManagement.Update)]
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

    [Permission(Policies.PetManagement.UpdatePet)]
    [HttpPatch("{volunteerId:guid}/petphoto/{petId:guid}/main")]
    public async Task<ActionResult<Guid>> ChangeMainPhoto(
        [FromServices] ChangePetMainPhotoHandler addPetPhotoHandler,
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromBody] ChangePetMainPhotoRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new ChangePetMainPhotoCommand(
            volunteerId,
            petId,
            request.FileName);

        var res = await addPetPhotoHandler
            .Handle(command, cancellationToken);

        if (res.IsFailure)
        {
            return res.Error.ToResponse();
        }

        return new ObjectResult(res.Value) { StatusCode = 201 };
    }

    [Permission(Policies.PetManagement.Update)]
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

    [Permission(Policies.PetManagement.UpdatePet)]
    [HttpPatch("{volunteerId:guid}/pet-status/{petId:guid}")]
    public async Task<ActionResult> UpdatePetStatus(
        [FromServices] UpdatePetStatusHandler updatePetStatusHandler,
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromForm] UpdatePetStatusRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdatePetStatusCommand(
            volunteerId,
            petId,
            request.PetStatus);

        var res = await updatePetStatusHandler.Handle(command, cancellationToken);

        if (res.IsFailure)
        {
            return res.Error.ToResponse();
        }

        return new ObjectResult(res.IsSuccess) { StatusCode = 201 };
    }

    [Permission(Policies.PetManagement.UpdatePet)]
    [HttpDelete("{volunteerId:guid}/petphoto/{petId:guid}")]
    public async Task<ActionResult<Guid>> DeletePetPhoto(
        [FromServices] DeletePetPhotoHandler deletePetPhotoHandler,
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromBody] DeletePetPhotoRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new DeletePetPhotoCommand(
            volunteerId,
            petId,
            request.BucketName,
            request.FileName);

        var res = await deletePetPhotoHandler
            .Handle(command, cancellationToken);

        if (res.IsFailure)
        {
            return res.Error.ToResponse();
        }

        return new ObjectResult(res.Value) { StatusCode = 201 };
    }

    [Permission(Policies.PetManagement.DeletePet)]
    [HttpDelete("{volunteerId:guid}/pet/{petId:guid}/force")]
    public async Task<ActionResult<Guid>> DeletePetForce(
        [FromServices] DeletePetHandler deletePetHandler,
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        CancellationToken cancellationToken = default)
    {
        var command = new DeletePetCommand(volunteerId, petId);

        var res = await deletePetHandler.Handle(command, cancellationToken);

        if (res.IsFailure)
        {
            return res.Error.ToResponse();
        }

        return new ObjectResult(res.Value) { StatusCode = 201 };
    }

    [Permission(Policies.PetManagement.Delete)]
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

    [Permission(Policies.PetManagement.DeletePet)]
    [HttpDelete("{volunteerId:guid}/pet/{petId:guid}/soft")]
    public async Task<ActionResult<Guid>> DeletePetSoft(
        [FromServices] DeletePetSoftHandler deletePetHandler,
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        CancellationToken cancellationToken = default)
    {
        var command = new DeletePetSoftCommand(volunteerId, petId);

        var res = await deletePetHandler.Handle(command, cancellationToken);

        if (res.IsFailure)
        {
            return res.Error.ToResponse();
        }

        return new ObjectResult(res.Value) { StatusCode = 201 };
    }
}