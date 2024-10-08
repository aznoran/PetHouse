﻿using CSharpFunctionalExtensions;
using PetHouse.SharedKernel.Constraints;
using PetHouse.SharedKernel.Other;

namespace PetHouse.SharedKernel.ValueObjects;

public record SocialNetwork
{
    public SocialNetwork()
    {
    }

    private SocialNetwork(string link, string name)
    {
        Link = link;
        Name = name;
    }

    public string Link { get; }


    public string Name { get; }

    public static Result<SocialNetwork, Error> Create(
        string link,
        string name)
    {
        if (link.Length > DefaultConstraints.MAX_LINK_LENGTH || string.IsNullOrWhiteSpace(link))
        {
            return Result.Failure<SocialNetwork, Error>(Errors.General.ValueIsRequired(nameof(link)));
        }

        if (name.Length > DefaultConstraints.MAX_NAME_LENGTH || string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<SocialNetwork, Error>(Errors.General.ValueIsRequired(nameof(name)));
        }

        return new SocialNetwork(
            link,
            name);
    }
}