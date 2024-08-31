using CSharpFunctionalExtensions;
using PetHouse.Domain.Models;
using PetHouse.Domain.Models.Other;
using PetHouse.Domain.Models.Volunteers.ValueObjects;
using PetHouse.Domain.Shared;
using PetHouse.Domain.ValueObjects;

namespace PetHouse.Application.Volunteers.CreateVolunteer;

public class CreateVolunteerHandler : ICreateVolunteerHandler
{
    private readonly IVolunteersRepository _repository;

    public CreateVolunteerHandler(IVolunteersRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Guid, Error>> Handle(CreateVolunteerRequest request, CancellationToken cancellationToken)
    {
        var fullName = FullName.Create(request.FullNameDto.Name, request.FullNameDto.Surname).Value;

        var description = Description.Create(request.Description).Value;

        var yearsOfExperience = YearsOfExperience.Create(request.YearsOfExperience).Value;

        var phoneNumber = PhoneNumber.Create(request.PhoneNumber).Value;

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
            description,
            yearsOfExperience,
            phoneNumber,
            socialNetworks,
            requisites).Value;

        return await _repository.Create(volunteer, cancellationToken);
    }
}