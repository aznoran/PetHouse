using Microsoft.AspNetCore.Mvc;
using PetHouse.API.Controllers.Shared;
using PetHouse.API.Extensions;
using PetHouse.Application.Files.Delete;
using PetHouse.Application.Files.Get;
using PetHouse.Application.Files.GetAll;
using PetHouse.Application.Files.Upload;

namespace PetHouse.API.Controllers;

[ApiController]
[Route("[controller]")]
public class FileController : ApplicationController
{
    /*[HttpPost]
    public async Task<IActionResult> Upload(
        IFormFile file,
        [FromServices] FileUploadHandler handler,
        CancellationToken cancellationToken)
    {
        await using var fileStream = file.OpenReadStream();

        var handlerRes = await handler.Handle(
            fileStream, 
            "photos", 
            cancellationToken);

        if (handlerRes.IsFailure)
        {
            return handlerRes.Error.ToResponse();
        }

        return Ok();
    }*/
    
    [HttpDelete]
    [Route("{fileName:guid}")]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid fileName,
        [FromServices] FileDeleteHandler handler,
        CancellationToken cancellationToken)
    {
        var handlerRes = await handler.Handle("photos", fileName.ToString(), cancellationToken);

        if (handlerRes.IsFailure)
        {
            return handlerRes.Error.ToResponse();
        }

        return Ok();
    }
    
    [HttpGet]
    [Route("{fileName:guid}")]
    public async Task<IActionResult> Get(
        [FromRoute] Guid fileName,
        [FromServices] FileGetHandler handler,
        CancellationToken cancellationToken)
    {
        var handlerRes = await handler.Handle("photos", fileName.ToString(), cancellationToken);

        if (handlerRes.IsFailure)
        {
            return handlerRes.Error.ToResponse();
        }

        return Ok(handlerRes.Value);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromServices] FileGetAllHandler handler,
        CancellationToken cancellationToken)
    {
        var handlerRes = await handler.Handle("photos",cancellationToken);

        if (handlerRes.IsFailure)
        {
            return handlerRes.Error.ToResponse();
        }

        return Ok(handlerRes.Value);
    }
}