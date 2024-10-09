using FluentValidation;
using PetHouse.Core.Validation;
using PetHouse.SharedKernel.Other;
using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.PetManagement.Application.Commands.UpdateSocialNetworks;

public class UpdateVolunteerSocialNetworksValidator : AbstractValidator<UpdateVolunteerSocialNetworksCommand>
{
    public UpdateVolunteerSocialNetworksValidator()
    {
        RuleFor(v => v.Id).NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleForEach(v => v.SocialNetworksDtos)
            .MustBeValueObject(sn => SocialNetwork.Create(sn.Link, sn.Name));
    }
}