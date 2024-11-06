using Microsoft.AspNetCore.Mvc;
using PetHouse.Accounts.Application.Commands.Login;
using PetHouse.Accounts.Application.Commands.Register;
using PetHouse.Accounts.Application.Queries.GetAccountById;
using PetHouse.Accounts.Contracts.Requests;
using PetHouse.Core.Extensions;
using PetHouse.Framework;

namespace PetHouse.Accounts.Presentation;

public class AccountsController : ApplicationController
{
    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetById(
        [FromServices] GetAccountByIdHandler handler,
        [FromRoute] Guid userId,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAccountByIdQuery();

        var result = await handler.Handle(query.GetQueryWithId(userId), cancellationToken);
        
        return Ok(result);
    }
    
    [HttpPost("registration")]
    public async Task<IActionResult> Register(
        [FromServices] RegisterHandler handler,
        [FromBody] RegisterRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new RegisterCommand(request.Email, request.Password);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToResponse();
        }
        
        return Ok();
    }
    
    [HttpPost("authorization")]
    public async Task<IActionResult> Login(
        [FromServices] LoginHandler handler,
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new LoginCommand(request.Email, request.Password);
        
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToResponse();
        }
        
        return Ok(result.Value);
    }
}