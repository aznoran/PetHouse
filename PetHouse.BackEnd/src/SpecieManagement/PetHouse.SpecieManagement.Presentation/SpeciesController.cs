using Microsoft.AspNetCore.Mvc;
using PetHouse.Core.Extensions;
using PetHouse.Framework;
using PetHouse.Framework.Authorization;
using PetHouse.SharedKernel.Constraints;
using PetHouse.SpecieManagement._Contracts.Species.Requests;
using PetHouse.SpecieManagement.Application.SpecieManagement.Commands.AddBreed;
using PetHouse.SpecieManagement.Application.SpecieManagement.Commands.Create;
using PetHouse.SpecieManagement.Application.SpecieManagement.Commands.Delete;
using PetHouse.SpecieManagement.Application.SpecieManagement.Commands.DeleteBreed;
using PetHouse.SpecieManagement.Application.SpecieManagement.Queries.GetAllWithPagination;
using PetHouse.SpecieManagement.Application.SpecieManagement.Queries.GetBreedById;

namespace PetHouse.SpecieManagement.Presentation;

public class SpeciesController : ApplicationController
{
    [Permission(Policies.SpeciesManagement.GetAll)]
    [HttpGet]
    public async Task<ActionResult> GetAll(
        [FromServices] GetAllSpeciesWithPaginationHandler getAllWithPaginationHandler,
        [FromQuery] GetAllSpeciesWithPaginationQuery query,
        CancellationToken cancellationToken = default)
    {
        var res = await getAllWithPaginationHandler.Handle(query, cancellationToken);

        return new ObjectResult(res) { StatusCode = 201 };
    }

    [Permission(Policies.SpeciesManagement.GetBreed)]
    [HttpGet("{specieId:guid}/breed")]
    public async Task<ActionResult> GetBreedById(
        [FromServices] GetBreedByIdHandler getBreedByIdHandler,
        [FromRoute] Guid speciesId,
        [FromQuery] GetBreedByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var res = await getBreedByIdHandler
            .Handle(query.GetQueryWithId(speciesId), cancellationToken);

        return new ObjectResult(res) { StatusCode = 201 };
    }

    [Permission(Policies.SpeciesManagement.Create)]
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] CreateSpecieHandler createSpecieHandler,
        [FromBody] CreateSpecieRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateSpecieCommand(request.Name);
        
        var res = await createSpecieHandler.Handle(command, cancellationToken);

        if (res.IsFailure)
        {
            return res.Error.ToResponse();
        }

        return new ObjectResult(res.Value) { StatusCode = 201 };
    }
    
    [Permission(Policies.SpeciesManagement.CreateBreed)]
    [HttpPost("{id:guid}/breed")]
    public async Task<ActionResult<Guid>> AddBreed(
        [FromServices] AddBreedHandler addBreedHandler,
        [FromRoute] Guid id,
        [FromBody] AddBreedRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new AddBreedCommand(id, request.Name);
        
        var res = await addBreedHandler.Handle(command, cancellationToken);

        if (res.IsFailure)
        {
            return res.Error.ToResponse();
        }

        return new ObjectResult(res.Value) { StatusCode = 201 };
    }
    
    [Permission(Policies.SpeciesManagement.Delete)]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Guid>> Delete(
        [FromServices] DeleteSpecieHandler deleteSpecieHandler,
        [FromRoute] Guid id,
        [FromBody] DeleteSpecieRequest deleteSpecieRequest,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteSpecieCommand(id);
        
        var res = await deleteSpecieHandler
            .Handle(command, cancellationToken);

        if (res.IsFailure)
        {
            return res.Error.ToResponse();
        }

        return new ObjectResult(res.Value) { StatusCode = 201 };
    }
    
    [Permission(Policies.SpeciesManagement.DeleteBreed)]
    [HttpDelete("{specieId:guid}/breed/{breedId:guid}")]
    public async Task<ActionResult<Guid>> DeleteBreed(
        [FromServices] DeleteBreedHandler deleteBreedHandler,
        [FromRoute] Guid specieId,
        [FromRoute] Guid breedId,
        [FromBody] DeleteBreedRequest deleteBreedRequest,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteBreedCommand(
            specieId,
            breedId);
        
        var res = await deleteBreedHandler
            .Handle(command, cancellationToken);

        if (res.IsFailure)
        {
            return res.Error.ToResponse();
        }

        return new ObjectResult(res.Value) { StatusCode = 201 };
    }
}