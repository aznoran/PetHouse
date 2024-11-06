using System.Data;
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
    private readonly IAccountManager _participantAccountManager;
    private readonly RoleManager<Role> _roleManager;

    public RegisterHandler(UserManager<User> userManager,
        ILogger<LoginHandler> logger,
        IValidator<RegisterCommand> validator,
        IUnitOfWork unitOfWork,
        IAccountManager participantAccountManager,
        RoleManager<Role> roleManager)
    {
        _userManager = userManager;
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _participantAccountManager = participantAccountManager;
        _roleManager = roleManager;
    }  
    
    public async Task<Result<Guid, ErrorList>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToErrorList();
        }

        var role = await _roleManager.FindByNameAsync(ParticipantAccount.PARTICIPANT);

        if (role is null)
        {
            return Errors.General.NotFound().ToErrorList();
        }

        var user = User.CreateParticipant(command.Email, command.Email, role);

        if (user.IsFailure)
        {
            return user.Error.ToErrorList();
        }
        
        using IDbTransaction transaction = await _unitOfWork.BeginTransaction(cancellationToken);

        try
        {
            var creatingResult = await _userManager.CreateAsync(user.Value, command.Password);

            if (!creatingResult.Succeeded)
            {
                return creatingResult.ToErrorList();
            }

            var fullName = FullName.Create(command.Email, command.Email);

            if (fullName.IsFailure)
            {
                return fullName.Error.ToErrorList();
            }

            var participantAccount = new ParticipantAccount()
                { Id = Guid.NewGuid(), User = user.Value, FullName = fullName.Value };

            await _participantAccountManager.AddParticipantAccount(participantAccount);

            _logger.LogInformation("Created account with id {Id}", user.Value.Id);

            await _unitOfWork.SaveChanges(cancellationToken);

            transaction.Commit();

            return user.Value.Id;
        }
        catch
        {
            _logger.LogError("Error while creating account with id {Id}", user.Value.Id);
            transaction.Rollback();
            return Errors.Database.TransactionFailed().ToErrorList();
        }
    }
}