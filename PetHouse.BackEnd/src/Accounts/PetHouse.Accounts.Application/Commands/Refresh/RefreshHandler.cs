using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetHouse.Accounts.Domain.Models;
using PetHouse.Core.Abstraction;
using PetHouse.Core.Extensions;
using PetHouse.Core.Providers;
using PetHouse.SharedKernel.Constraints;
using PetHouse.SharedKernel.Other;
using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.Accounts.Application.Commands.Refresh;

public class RefreshHandler : ICommandHandler<RefreshCommand, JwtTokenResult>
{
    private readonly IRefreshSessionManager _refreshSessionManager;
    private readonly ILogger<RefreshHandler> _logger;
    private readonly IValidator<RefreshCommand> _validator;
    private readonly ITokenProvider _tokenProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;

    public RefreshHandler(IRefreshSessionManager refreshSessionManager,
        ILogger<RefreshHandler> logger,
        IValidator<RefreshCommand> validator, 
        ITokenProvider tokenProvider,
        [FromKeyedServices(ModuleNames.Accounts)]IUnitOfWork unitOfWork,
        UserManager<User> userManager)
    {
        _refreshSessionManager = refreshSessionManager;
        _logger = logger;
        _validator = validator;
        _tokenProvider = tokenProvider;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task<Result<JwtTokenResult, ErrorList>> Handle(RefreshCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToErrorList();
        }

        var refreshSession = await _refreshSessionManager
            .GetRefreshSesssionByToken(command.RefreshToken, cancellationToken);

        if (refreshSession.IsFailure)
        {
            return refreshSession.Error.ToErrorList();
        }

        var refreshSessionCopy = new RefreshSession()
        {
            ExpiresAt = refreshSession.Value.ExpiresAt,
            UserId = refreshSession.Value.UserId
        };

        _refreshSessionManager.DeleteRefreshSession(refreshSession.Value, cancellationToken);

        await _unitOfWork.SaveChanges(cancellationToken);
        
        if (refreshSessionCopy.ExpiresAt < DateTime.UtcNow)
        {
            return Errors.Tokens.RefreshTokenExpired().ToErrorList();
        }

        var user = await _userManager.FindByIdAsync(refreshSessionCopy.UserId.ToString());
        
        var accessToken = _tokenProvider.GenerateAccessToken(user!, cancellationToken);
        var refreshToken = await _tokenProvider.GenerateRefreshToken(user!, cancellationToken);
        
        _logger.LogInformation("Refreshed tokens successful");

        return new JwtTokenResult(accessToken, refreshToken);
    }
}