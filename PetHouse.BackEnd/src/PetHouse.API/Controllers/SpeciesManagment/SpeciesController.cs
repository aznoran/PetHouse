using Microsoft.AspNetCore.Mvc;
using PetHouse.API.Controllers.Shared;
using PetHouse.API.Controllers.SpeciesManagment.Requests;
using PetHouse.API.Extensions;
using PetHouse.Application.SpecieManagement.Commands.AddBreed;
using PetHouse.Application.SpecieManagement.Commands.Create;
using PetHouse.Application.SpecieManagement.Commands.Delete;
using PetHouse.Application.SpecieManagement.Commands.DeleteBreed;
using PetHouse.Application.SpecieManagement.Queries.GetAllWithPagination;
using PetHouse.Application.SpecieManagement.Queries.GetBreedById;

namespace PetHouse.API.Controllers.SpeciesManagment;

public class SpeciesController : ApplicationController
{
    [HttpGet]
    public async Task<ActionResult> GetAll(
        [FromServices] GetAllSpeciesWithPaginationHandler getAllWithPaginationHandler,
        [FromQuery] GetAllSpeciesWithPaginationQuery query,
        CancellationToken cancellationToken = default)
    {
        var res = await getAllWithPaginationHandler.Handle(query, cancellationToken);

        return new ObjectResult(res) { StatusCode = 201 };
    }

    [HttpGet("{specieId:guid}/breed")]
    public async Task<ActionResult> GetBreedById(
        [FromServices] GetBreedByIdHandler getBreedByIdHandler,
        [FromRoute] Guid speciesId,
        [FromQuery] GetBreedByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var res = await getBreedByIdHandler
            .Handle(query.GetQueryWithId(speciesId, query), cancellationToken);

        return new ObjectResult(res) { StatusCode = 201 };
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] CreateSpecieHandler createSpecieHandler,
        [FromBody] CreateSpecieRequest createSpecieRequest,
        CancellationToken cancellationToken = default)
    {
        var res = await createSpecieHandler.Handle(createSpecieRequest.ToCommand(), cancellationToken);

        if (res.IsFailure)
        {
            return res.Error.ToResponse();
        }

        return new ObjectResult(res.Value) { StatusCode = 201 };
    }
    
    [HttpPost("{id:guid}/breed")]
    public async Task<ActionResult<Guid>> AddBreed(
        [FromServices] AddBreedHandler addBreedHandler,
        [FromRoute] Guid id,
        [FromBody] AddBreedRequest addBreedRequest,
        CancellationToken cancellationToken = default)
    {
        var res = await addBreedHandler.Handle(addBreedRequest.ToCommand(id), cancellationToken);

        if (res.IsFailure)
        {
            return res.Error.ToResponse();
        }

        return new ObjectResult(res.Value) { StatusCode = 201 };
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Guid>> DeleteSpecie(
        [FromServices] DeleteSpecieHandler deleteSpecieHandler,
        [FromRoute] Guid id,
        [FromBody] DeleteSpecieRequest deleteSpecieRequest,
        CancellationToken cancellationToken = default)
    {
        var res = await deleteSpecieHandler
            .Handle(deleteSpecieRequest.ToCommand(id), cancellationToken);

        if (res.IsFailure)
        {
            return res.Error.ToResponse();
        }

        return new ObjectResult(res.Value) { StatusCode = 201 };
    }
    
    [HttpDelete("{specieId:guid}/breed/{breedId:guid}")]
    public async Task<ActionResult<Guid>> AddBreed(
        [FromServices] DeleteBreedHandler deleteBreedHandler,
        [FromRoute] Guid specieId,
        [FromRoute] Guid breedId,
        [FromBody] DeleteBreedRequest deleteBreedRequest,
        CancellationToken cancellationToken = default)
    {
        var res = await deleteBreedHandler
            .Handle(deleteBreedRequest.ToCommand(specieId, breedId), cancellationToken);

        if (res.IsFailure)
        {
            return res.Error.ToResponse();
        }

        return new ObjectResult(res.Value) { StatusCode = 201 };
    }
}