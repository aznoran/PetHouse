using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHouse.Application.Extensions;
using PetHouse.Domain.Models.Other;
using PetHouse.Domain.Shared;
using PetHouse.Domain.ValueObjects;
using PetHouse.Infrastructure;

namespace PetHouse.Application.Volunteers.UpdateSocialNetworks;

public class UpdateVolunteerSocialNetworksHandler : IUpdateVolunteerSocialNetworksHandler
{
    private readonly IVolunteersRepository _repository;
    private readonly ILogger<UpdateVolunteerSocialNetworksHandler> _logger;
    private readonly IValidator<UpdateVolunteerSocialNetworksCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateVolunteerSocialNetworksHandler(
        IVolunteersRepository repository,
        ILogger<UpdateVolunteerSocialNetworksHandler> logger,
        IValidator<UpdateVolunteerSocialNetworksCommand> validator,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateVolunteerSocialNetworksCommand command,
        CancellationToken cancellationToken)
    {
        var validationRes = await _validator.ValidateAsync(command, cancellationToken);

        if (validationRes.IsValid == false)
        {
            return validationRes.ToList();
        }

        //await _unitOfWork.BeginTransaction(cancellationToken);

        var volunteerResult = await _repository.GetById(command.Id, cancellationToken);

        if (volunteerResult.IsFailure)
        {
            return Errors.General.NotFound(command.Id).ToErrorList();
        }

        var socialNetworks = command.SocialNetworksDtos
            .Select(sn => SocialNetwork
                .Create(sn.Link, sn.Name).Value)
            .ToList();

        volunteerResult.Value.UpdateSocialNetworks(new SocialNetworkInfo(socialNetworks));

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Updated {Volunteer} social networks with id {Id}", volunteerResult.Value, command.Id);

        return command.Id;
    }
}