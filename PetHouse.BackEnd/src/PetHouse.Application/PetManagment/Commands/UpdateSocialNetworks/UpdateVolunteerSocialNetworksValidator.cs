using FluentValidation;
using PetHouse.Application.Validation;
using PetHouse.Domain.Shared.Other;
using PetHouse.Domain.Shared.ValueObjects;

namespace PetHouse.Application.PetManagment.Commands.UpdateSocialNetworks;

public class UpdateVolunteerSocialNetworksValidator : AbstractValidator<UpdateVolunteerSocialNetworksCommand>
{
    public UpdateVolunteerSocialNetworksValidator()
    {
        RuleFor(v => v.Id).NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleForEach(v => v.SocialNetworksDtos)
            .MustBeValueObject(sn => SocialNetwork.Create(sn.Link, sn.Name));
    }
}