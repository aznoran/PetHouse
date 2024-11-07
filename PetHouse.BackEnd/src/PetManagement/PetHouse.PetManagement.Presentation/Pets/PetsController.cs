using Microsoft.AspNetCore.Mvc;
using PetHouse.Framework;
using PetHouse.Framework.Authorization;
using PetHouse.PetManagement.Application.Queries.GetAllPetsWithPaginationAndFilter;
using PetHouse.PetManagement.Application.Queries.GetPetById;

namespace PetHouse.PetManagement.Presentation.Pets;

public class PetsController : ApplicationController
{
    [Permission(Policies.PetManagement.GetPet)]
    [HttpGet]
    public async Task<ActionResult<Guid>> GetAllPets(
        [FromServices] GetAllPetsWithPaginationAndFilterHandler getAllWithPaginationHandler,
        [FromQuery] GetAllPetsWithPaginationAndFilterQuery query,
        CancellationToken cancellationToken = default)
    {
        var res = await getAllWithPaginationHandler.Handle(query, cancellationToken);
        
        return new ObjectResult(res) { StatusCode = 201 };
    }
    
    [Permission(Policies.PetManagement.GetPet)]
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