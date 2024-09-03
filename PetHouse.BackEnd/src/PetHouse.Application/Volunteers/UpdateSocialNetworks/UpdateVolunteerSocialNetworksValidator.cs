using FluentValidation;
using PetHouse.Application.Validation;
using PetHouse.Domain.Models.Other;
using PetHouse.Domain.Shared;

namespace PetHouse.Application.Volunteers.UpdateSocialNetworks;

public class UpdateVolunteerSocialNetworksValidator : AbstractValidator<UpdateVolunteerSocialNetworksRequest>
{
    public UpdateVolunteerSocialNetworksValidator()
    {
        RuleFor(v => v.Id).NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleForEach(v => v.SocialNetworksDtos)
            .MustBeValueObject(sn => SocialNetwork.Create(sn.Link, sn.Name));
    }
}