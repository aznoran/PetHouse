using Microsoft.EntityFrameworkCore;
using PetHouse.Domain.PetManagment.Aggregate;
using PetHouse.Domain.PetManagment.Entities;
using PetHouse.Domain.PetManagment.ValueObjects;
using PetHouse.Domain.Shared.Id;
using PetHouse.Domain.Shared.ValueObjects;
// ReSharper disable InconsistentNaming

namespace PetHouse.Infrastructure.Extensions;

public static class EfCoreModelBuilder
{
    private const int MINIMUM_VOLUNTEERS = 10;
    private const int MAXIMUM_VOLUNTEERS = 20;
    private const int MINIMUM_PETS = 1;
    private const int MAXIMUM_PETS = 5;
    private const int MINIMUM_REQUISITES = 0;
    private const int MAXIMUM_REQUISITES = 5;
    private const int MINIMUM_SOCIALNETWORKS = 0;
    private const int MAXIMUM_SOCIALNETWORKS = 5;
    private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private const string colorchars = "0123456789abcdef";
    private const string digits = "0123456789";

    private static readonly Random Random = new Random();

    public static ModelBuilder HasStartingData(this ModelBuilder modelBuilder)
    {
        var volunteers = new List<Volunteer>();

        var pets = new List<Pet>();

        var volunteersNumber = Random.Next(MINIMUM_VOLUNTEERS, MAXIMUM_VOLUNTEERS);

        for (var i = 0; i < volunteersNumber; ++i)
        {
            var name = new string(Enumerable.Repeat(chars, 10)
                .Select(s => s[Random.Next(s.Length)]).ToArray());

            var surname = new string(Enumerable.Repeat(chars, 10)
                .Select(s => s[Random.Next(s.Length)]).ToArray());

            var email = new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[Random.Next(s.Length)]).ToArray());

            var emailExtension = new string(Enumerable.Repeat(chars, 3)
                .Select(s => s[Random.Next(s.Length)]).ToArray());

            var description = new string(Enumerable.Repeat(chars, 30)
                .Select(s => s[Random.Next(s.Length)]).ToArray());

            var yearsOfExperience = Random.Next(1, 20);

            var phoneNumber = "8" + new string(Enumerable.Repeat(digits, 10)
                .Select(s => s[Random.Next(s.Length)]).ToArray());

            var reqNumber = Random.Next(MINIMUM_REQUISITES,MAXIMUM_REQUISITES);

            var requisites = new List<Requisite>();

            for (var j = 0; j < reqNumber; ++j)
            {
                var reqName = new string(Enumerable.Repeat(chars, 10)
                    .Select(s => s[Random.Next(s.Length)]).ToArray());

                var reqDescription = new string(Enumerable.Repeat(chars, 8)
                    .Select(s => s[Random.Next(s.Length)]).ToArray());

                requisites.Add(Requisite.Create(reqName, reqDescription).Value);
            }

            var snNumber = Random.Next(MINIMUM_SOCIALNETWORKS,MAXIMUM_SOCIALNETWORKS);

            var socialNetworks = new List<SocialNetwork>();

            for (int j = 0; j < snNumber; ++j)
            {
                var snLink = new string(Enumerable.Repeat(chars, 10)
                    .Select(s => s[Random.Next(s.Length)]).ToArray());

                var snName = new string(Enumerable.Repeat(chars, 8)
                    .Select(s => s[Random.Next(s.Length)]).ToArray());

                socialNetworks.Add(SocialNetwork.Create(snLink, snName).Value);
            }

            var volunteer = Volunteer.Create(VolunteerId.NewId,
                FullName.Create(name, surname).Value,
                Email.Create($"{email}@{emailExtension}.com").Value,
                Description.Create(description).Value,
                YearsOfExperience.Create(yearsOfExperience).Value,
                PhoneNumber.Create(phoneNumber).Value,
                socialNetworks,
                requisites).Value;

            var petNumber = Random.Next(MINIMUM_PETS, MAXIMUM_PETS);

            for (int j = 0; j < petNumber; ++j)
            {
                name = new string(Enumerable.Repeat(chars, 10)
                    .Select(s => s[Random.Next(s.Length)]).ToArray());

                var petIdentifier = PetIdentifier
                    .Create(Guid.NewGuid(), Guid.NewGuid());

                var color = "#" + new string(Enumerable.Repeat(colorchars, 3)
                    .Select(s => s[Random.Next(s.Length)]).ToArray());

                var healthInfo = new string(Enumerable.Repeat(chars, 8)
                    .Select(s => s[Random.Next(s.Length)]).ToArray());

                var weight = Random.Next(30, 70);

                var height = Random.Next(30, 70);

                var date = DateTime.UtcNow.AddHours(Random.Next(0, 100));

                var city = new string(Enumerable.Repeat(chars, 8)
                    .Select(s => s[Random.Next(s.Length)]).ToArray());

                var street = new string(Enumerable.Repeat(chars, 8)
                    .Select(s => s[Random.Next(s.Length)]).ToArray());

                var country = new string(Enumerable.Repeat(chars, 8)
                    .Select(s => s[Random.Next(s.Length)]).ToArray());

                phoneNumber = "8" + new string(Enumerable.Repeat(digits, 10)
                    .Select(s => s[Random.Next(s.Length)]).ToArray());

                reqNumber = Random.Next(MINIMUM_REQUISITES,MAXIMUM_REQUISITES);

                requisites = new List<Requisite>();

                for (int k = 0; k < reqNumber; ++k)
                {
                    var reqName = new string(Enumerable.Repeat(chars, 10)
                        .Select(s => s[Random.Next(s.Length)]).ToArray());

                    var reqDescription = new string(Enumerable.Repeat(chars, 8)
                        .Select(s => s[Random.Next(s.Length)]).ToArray());

                    requisites.Add(Requisite.Create(reqName, reqDescription).Value);
                }

                var petStatus = PetStatusInfo.Create(Random.Next(1, 3));

                volunteer.AddPet(
                    Name.Create(name).Value,
                    petIdentifier.Value,
                    Description.Create(description).Value,
                    PetInfo.Create(
                        color,
                        healthInfo,
                        weight,
                        height,
                        Random.Next(1) == 1,
                        Random.Next(1) == 1,
                        date).Value,
                    Address.Create(city, street, country).Value,
                    PhoneNumber.Create(phoneNumber).Value,
                    requisites,
                    petStatus.Value,
                    date);
            }

            volunteers.Add(volunteer);
            
            pets.AddRange(volunteer.Pets!);
        }

        modelBuilder.Entity<Volunteer>().HasData(volunteers);

        //modelBuilder.Entity<Pet>().HasData(pets);

        return modelBuilder;
    }
}