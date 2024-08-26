using PetHouse.Domain.Models;
using PetHouse.Domain.Models.Other;
using PetHouse.Domain.ValueObjects;

namespace PetHouse.Application.Volunteers.CreateVolunteer;

public class CreateVolunteerHandler
{
    
    private readonly IVolunteersRepository _repository;

    public CreateVolunteerHandler(IVolunteersRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Guid> Handle(CreateVolunteerRequest request, CancellationToken cancellationToken)
    {
        VolunteerProfile volunteerProfile = VolunteerProfile.Create(
            request.VolunteerProfileDto.FullName,
            request.VolunteerProfileDto.Description,
            request.VolunteerProfileDto.YearsOfExperience,
            request.VolunteerProfileDto.CountOfPetsFoundHome,
            request.VolunteerProfileDto.CountOfPetsLookingForHome,
            request.VolunteerProfileDto.CountOfPetsOnTreatment,
            request.VolunteerProfileDto.PhoneNumber
            );

        SocialNetworkInfo socialNetworkInfo = new SocialNetworkInfo(request.SocialNetworksDto.Select(
            sn => SocialNetwork.Create(sn.Reference, sn.Name)).ToList());

        RequisiteInfo requisiteInfo = new RequisiteInfo(request.RequisiteDto.Select(
            r => Requisite.Create(r.Name, r.Description)).ToList());
        
        
        Volunteer volunteer = Volunteer.Create(VolunteerId.NewId, volunteerProfile, socialNetworkInfo, requisiteInfo, null);

        return await _repository.Create(volunteer, cancellationToken);
    }
}