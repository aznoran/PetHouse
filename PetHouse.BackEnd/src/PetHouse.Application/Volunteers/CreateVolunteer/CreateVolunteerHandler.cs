using CSharpFunctionalExtensions;
using PetHouse.Domain.Models;
using PetHouse.Domain.Models.Other;
using PetHouse.Domain.Shared;
using PetHouse.Domain.ValueObjects;

namespace PetHouse.Application.Volunteers.CreateVolunteer;
public class CreateVolunteerHandler
{
    private readonly IVolunteersRepository _repository;

    public CreateVolunteerHandler(IVolunteersRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Result<Guid, Error>> Handle(CreateVolunteerRequest request, CancellationToken cancellationToken)
    {
        var volunteerProfile = VolunteerProfile.Create(
            request.VolunteerProfileDto.FullName,
            request.VolunteerProfileDto.Description,
            request.VolunteerProfileDto.YearsOfExperience,
            request.VolunteerProfileDto.CountOfPetsFoundHome,
            request.VolunteerProfileDto.CountOfPetsLookingForHome,
            request.VolunteerProfileDto.CountOfPetsOnTreatment,
            request.VolunteerProfileDto.PhoneNumber
            );
        if (volunteerProfile.IsFailure)
        {
            return Result.Failure<Guid, Error>(volunteerProfile.Error);
        }
        
        //Валидация социальных сетей, как я увидел в чате - в дальнейшем я ее изменю на автовалидацию,
        //а поэтому на данный момент я решил не дублировать ее для реквезитов и просто вернуть те,
        //которые являются верными без возвращения ошибок
        var socialNetworkResults = request.SocialNetworksDto
            .Select(sn => SocialNetwork.Create(sn.Link, sn.Name))
            .ToList();
        var failedNetworks = socialNetworkResults
            .Where(result => result.IsFailure)
            .Select(r => r.Error)
            .ToList();
        if (failedNetworks.Count > 0)
        {
            return Result.Failure<Guid, Error>(failedNetworks.FirstOrDefault()!);
        }

        var socialNetworkInfo = new SocialNetworkInfo(socialNetworkResults.Select(sn => sn.Value));
        
        RequisiteInfo requisiteInfo = new RequisiteInfo(request.RequisiteDto.Select(
            r => Requisite.Create(
                r.Name, 
                r.Description))
            .Where(r => r.IsFailure == false)
            .Select(r => r.Value)
            .ToList());
        
        Volunteer volunteer = Volunteer.Create(
            VolunteerId.NewId, 
            volunteerProfile.Value, 
            socialNetworkInfo,
            requisiteInfo).Value;

        return await _repository.Create(volunteer, cancellationToken);
    }
}