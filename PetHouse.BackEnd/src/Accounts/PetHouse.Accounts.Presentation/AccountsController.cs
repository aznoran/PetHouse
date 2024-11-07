using Microsoft.AspNetCore.Mvc;
using PetHouse.Accounts.Application.Commands.Login;
using PetHouse.Accounts.Application.Commands.Refresh;
using PetHouse.Accounts.Application.Commands.Register;
using PetHouse.Accounts.Application.Queries.GetAccountById;
using PetHouse.Accounts.Contracts.Requests;
using PetHouse.Core.Extensions;
using PetHouse.Framework;

namespace PetHouse.Accounts.Presentation;

public class AccountsController : ApplicationController
{
    private const string REFRESH_TOKEN = "refresh_token";
    
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
        
        HttpContext.Response.Cookies.Append(REFRESH_TOKEN, result.Value.RefreshToken.ToString());
        
        return Ok(result.Value);
    }
    
    [HttpPost("session_refreshment")]
    public async Task<IActionResult> Refresh(
        [FromServices] RefreshHandler handler,
        CancellationToken cancellationToken = default)
    {
        if (!HttpContext.Request.Cookies.TryGetValue(REFRESH_TOKEN, out var refreshToken))
        {
            return Unauthorized();
        }

        var command = new RefreshCommand(Guid.Parse(refreshToken));
        
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToResponse();
        }
        
        HttpContext.Response.Cookies.Append(REFRESH_TOKEN, result.Value.RefreshToken.ToString());
        
        return Ok(result.Value);
    }
}