using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetHouse.Accounts.Application.Commands.Login;
using PetHouse.Accounts.Domain.Models;
using PetHouse.Core.Abstraction;
using PetHouse.Core.Extensions;
using PetHouse.SharedKernel.Other;
using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.Accounts.Application.Commands.Register;

public class RegisterHandler : ICommandHandler<RegisterCommand, Guid>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<LoginHandler> _logger;
    private readonly IValidator<RegisterCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterHandler(UserManager<User> userManager,
        ILogger<LoginHandler> logger,
        IValidator<RegisterCommand> validator,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }  
    
    public async Task<Result<Guid, ErrorList>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToErrorList();
        }

        User user = new User()
        {
            Id = Guid.NewGuid(),
            UserName = command.Email,
            Email = command.Email
        };

        var creatingResult = await _userManager.CreateAsync(user, command.Password);

        if (!creatingResult.Succeeded)
        {
            return creatingResult.ToErrorList();
        }
        
        _logger.LogInformation("Created account with id {Id}", user.Id);

        await _unitOfWork.SaveChanges(cancellationToken);

        return user.Id;
    }
}