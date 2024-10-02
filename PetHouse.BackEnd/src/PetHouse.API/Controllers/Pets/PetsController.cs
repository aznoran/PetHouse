using Microsoft.AspNetCore.Mvc;
using PetHouse.API.Controllers.Shared;
using PetHouse.Application.PetManagement.Queries.GetAllPetsWithPaginationAndFilter;
using PetHouse.Application.PetManagement.Queries.GetPetById;

namespace PetHouse.API.Controllers.Pets;

public class PetsController : ApplicationController
{
    [HttpGet]
    public async Task<ActionResult<Guid>> GetAllPets(
        [FromServices] GetAllPetsWithPaginationAndFilterHandler getAllWithPaginationHandler,
        [FromQuery] GetAllPetsWithPaginationAndFilterQuery query,
        CancellationToken cancellationToken = default)
    {
        var res = await getAllWithPaginationHandler.Handle(query, cancellationToken);
        
        return new ObjectResult(res) { StatusCode = 201 };
    }
    

    [HttpGet("{petId:guid}")]
    public async Task<ActionResult<Guid>> GetPet(
        [FromServices] GetPetByIdHandler getPetByIdHandler,
        [FromRoute] Guid petId,
        [FromQuery] GetPetByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var res = await getPetByIdHandler.Handle(
            query.GetQueryWithId(petId), cancellationToken);
        
        return new ObjectResult(res) { StatusCode = 201 };
    }
}