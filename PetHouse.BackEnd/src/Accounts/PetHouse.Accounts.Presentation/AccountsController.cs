using Microsoft.AspNetCore.Mvc;
using PetHouse.Accounts.Application.Commands.Login;
using PetHouse.Accounts.Application.Commands.Register;
using PetHouse.Accounts.Contracts.Requests;
using PetHouse.Core.Extensions;
using PetHouse.Framework;

namespace PetHouse.Accounts.Presentation;

public class AccountsController : ApplicationController
{
    /*[HttpPost("registration")]
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
    }*/
    
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