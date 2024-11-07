using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetHouse.Accounts.Domain.Models;
using PetHouse.Core.Abstraction;
using PetHouse.Core.Extensions;
using PetHouse.SharedKernel.Other;
using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.Accounts.Application.Commands.Login;

public class LoginHandler : ICommandHandler<LoginCommand, JwtTokenResult>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<LoginHandler> _logger;
    private readonly IValidator<LoginCommand> _validator;
    private readonly ITokenProvider _tokenProvider;
    private readonly IRefreshSessionManager _refreshSessionManager;

    public LoginHandler(UserManager<User> userManager,
        ILogger<LoginHandler> logger,
        IValidator<LoginCommand> validator, 
        ITokenProvider tokenProvider,
        IRefreshSessionManager refreshSessionManager)
    {
        _userManager = userManager;
        _logger = logger;
        _validator = validator;
        _tokenProvider = tokenProvider;
        _refreshSessionManager = refreshSessionManager;
    }  
    
    public async Task<Result<JwtTokenResult, ErrorList>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToErrorList();
        }

        var user = await _userManager.FindByEmailAsync(command.Email);

        if (user is null)
        {
            return Errors.General.NotFound().ToErrorList();
        }

        if (await _userManager.CheckPasswordAsync(user!, command.Password) == false)
        {
            return Errors.Accounts.InvalidCredentials().ToErrorList();
        }

        var accessToken = _tokenProvider.GenerateAccessToken(user, cancellationToken);
        
        var refreshSession = await _refreshSessionManager
            .GetRefreshSesssionByUserId(user.Id, cancellationToken);

        if (refreshSession.IsSuccess)
        {
            _refreshSessionManager.DeleteRefreshSession(refreshSession.Value, cancellationToken);
        }
        
        var refreshToken = await _tokenProvider.GenerateRefreshToken(user, cancellationToken);
        
        _logger.LogInformation("Logged successful with id {Id}", user.Id);

        return new JwtTokenResult(accessToken, refreshToken);
    }
}