using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetHouse.Domain.Models;
using PetHouse.Domain.Models.Other;
using PetHouse.Domain.Models.Volunteers.ValueObjects;
using PetHouse.Domain.Shared;
using PetHouse.Domain.ValueObjects;

namespace PetHouse.Application.Volunteers.Create;

public class CreateVolunteerHandler : ICreateVolunteerHandler
{
    private readonly IVolunteersRepository _repository;
    private readonly ILogger<CreateVolunteerHandler> _logger;
    public CreateVolunteerHandler(IVolunteersRepository repository, ILogger<CreateVolunteerHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(CreateVolunteerRequest request, CancellationToken cancellationToken)
    {
        var fullName = FullName.Create(request.FullNameDto.Name, request.FullNameDto.Surname).Value;

        var description = Description.Create(request.Description).Value;

        var yearsOfExperience = YearsOfExperience.Create(request.YearsOfExperience).Value;

        var email = Email.Create(request.Email).Value;

        var emailRes = await _repository.GetByEmail(email);
        
        if (emailRes.IsSuccess)
        {
            return Errors.Volunteer.AlreadyExists(email.Value, nameof(email));
        }
        
        var phoneNumber = PhoneNumber.Create(request.PhoneNumber).Value;

        var phoneNumberRes = await _repository.GetByPhoneNumber(phoneNumber);
        
        if (phoneNumberRes.IsSuccess)
        {
            return Errors.Volunteer.AlreadyExists(phoneNumber.Value, nameof(phoneNumber));
        }
       
        var socialNetworks = new SocialNetworkInfo(request.SocialNetworksDto
            .Select(sn => SocialNetwork.Create(sn.Link, sn.Name))
            .ToList().Select(sn => sn.Value));

        var requisites = new RequisiteInfo(request.RequisiteDto.Select(
                r => Requisite.Create(
                    r.Name,
                    r.Description)).Select(r => r.Value)
            .ToList());

        Volunteer volunteer = Volunteer.Create(
            VolunteerId.NewId,
            fullName,
            email,
            description,
            yearsOfExperience,
            phoneNumber,
            socialNetworks,
            requisites).Value;

        _logger.LogInformation("Created Volunteer {FullName} with id {VolunteerId}", fullName, volunteer.Id);
        
        return await _repository.Create(volunteer, cancellationToken);
    }
}