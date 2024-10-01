using CSharpFunctionalExtensions;
using FluentAssertions;
using PetHouse.Domain.PetManagement.Aggregate;
using PetHouse.Domain.PetManagement.Enums;
using PetHouse.Domain.PetManagement.ValueObjects;
using PetHouse.Domain.Shared;
using PetHouse.Domain.Shared.Id;
using PetHouse.Domain.Shared.Other;
using PetHouse.Domain.Shared.ValueObjects;

namespace PetHouse.Domain.UnitTests;

public class VolunteerTest
{
    [Fact]
    public void Test_Move_Pet_To_Any_Position_With_No_Validation_Error()
    {
        //arrange
        FullName fullName = FullName.Create("test", "test").Value;
        Email email = Email.Create("test@gmail.com").Value;
        Description description = Description.Create("test").Value;
        YearsOfExperience yearsOfExperience = YearsOfExperience.Create(12).Value;
        PhoneNumber phoneNumber = PhoneNumber.Create("89251111111").Value;
        var socialNetworks = new[]
            { SocialNetwork.Create("test", "test").Value };
        var requisites = new[]
            { Requisite.Create("test", "test").Value };

        var volunteer = Volunteer.Create(VolunteerId.NewId,
            fullName,
            email,
            description,
            yearsOfExperience,
            phoneNumber,
            socialNetworks,
            requisites);

        Name name = Name.Create("test").Value;
        PetIdentifier petIdentifier = PetIdentifier.Create(Guid.NewGuid(), Guid.NewGuid()).Value;
        PetInfo petInfo = PetInfo.Create("#fff",
            "good",
            20,
            20,
            true,
            true,
            System.DateTime.Now).Value;
        Address address = Address.Create("test", "test", "test").Value;
        PetStatus petStatus = PetStatus.LookingForHome;
        DateTime createdAt = DateTime.Now;

        volunteer.Value.AddPet(name,
            petIdentifier,
            description,
            petInfo,
            address,
            phoneNumber,
            requisites,
            petStatus,
            createdAt);
        
        volunteer.Value.AddPet(name,
            petIdentifier,
            description,
            petInfo,
            address,
            phoneNumber,
            requisites,
            petStatus,
            createdAt);
        
        volunteer.Value.AddPet(name,
            petIdentifier,
            description,
            petInfo,
            address,
            phoneNumber,
            requisites,
            petStatus,
            createdAt);
        
        volunteer.Value.AddPet(name,
            petIdentifier,
            description,
            petInfo,
            address,
            phoneNumber,
            requisites,
            petStatus,
            createdAt);
        
        volunteer.Value.AddPet(name,
            petIdentifier,
            description,
            petInfo,
            address,
            phoneNumber,
            requisites,
            petStatus,
            createdAt);

        var firstPet = volunteer.Value.Pets[0];
        var secondPet = volunteer.Value.Pets[1];
        var thirdPet = volunteer.Value.Pets[2];
        var fourthPet = volunteer.Value.Pets[3];
        var fifthPet = volunteer.Value.Pets[4];
        
        //act
        var position = Position.Create(1).Value;

        var moveRes = volunteer.Value.MovePet(thirdPet.Id, position).IsSuccess;

        //assert
        moveRes.Should().BeTrue();
        firstPet.Position.Should().Be(Position.Create(2).Value);
        secondPet.Position.Should().Be(Position.Create(3).Value);
        thirdPet.Position.Should().Be(Position.Create(1).Value);
        fourthPet.Position.Should().Be(Position.Create(4).Value);
        fifthPet.Position.Should().Be(Position.Create(5).Value);
    }
    
    [Fact]
    public void Test_Move_Pet_To_Last_Position_With_No_Validation_Error()
    {
        //arrange
        FullName fullName = FullName.Create("test", "test").Value;
        Email email = Email.Create("test@gmail.com").Value;
        Description description = Description.Create("test").Value;
        YearsOfExperience yearsOfExperience = YearsOfExperience.Create(12).Value;
        PhoneNumber phoneNumber = PhoneNumber.Create("89251111111").Value;
        var socialNetworks = new[]
            { SocialNetwork.Create("test", "test").Value };
        var requisites = new[]
            { Requisite.Create("test", "test").Value };

        var volunteer = Volunteer.Create(VolunteerId.NewId,
            fullName,
            email,
            description,
            yearsOfExperience,
            phoneNumber,
            socialNetworks,
            requisites);

        Name name = Name.Create("test").Value;
        PetIdentifier petIdentifier = PetIdentifier.Create(Guid.NewGuid(), Guid.NewGuid()).Value;
        PetInfo petInfo = PetInfo.Create("#fff",
            "good",
            20,
            20,
            true,
            true,
            System.DateTime.Now).Value;
        Address address = Address.Create("test", "test", "test").Value;
        PetStatus petStatus = PetStatus.LookingForHome;
        DateTime createdAt = DateTime.Now;

        volunteer.Value.AddPet(name,
            petIdentifier,
            description,
            petInfo,
            address,
            phoneNumber,
            requisites,
            petStatus,
            createdAt);
        
        volunteer.Value.AddPet(name,
            petIdentifier,
            description,
            petInfo,
            address,
            phoneNumber,
            requisites,
            petStatus,
            createdAt);
        
        volunteer.Value.AddPet(name,
            petIdentifier,
            description,
            petInfo,
            address,
            phoneNumber,
            requisites,
            petStatus,
            createdAt);
        
        volunteer.Value.AddPet(name,
            petIdentifier,
            description,
            petInfo,
            address,
            phoneNumber,
            requisites,
            petStatus,
            createdAt);
        
        volunteer.Value.AddPet(name,
            petIdentifier,
            description,
            petInfo,
            address,
            phoneNumber,
            requisites,
            petStatus,
            createdAt);

        var firstPet = volunteer.Value.Pets[0];
        var secondPet = volunteer.Value.Pets[1];
        var thirdPet = volunteer.Value.Pets[2];
        var fourthPet = volunteer.Value.Pets[3];
        var fifthPet = volunteer.Value.Pets[4];
        
        //act
        var moveRes = volunteer.Value.MovePetToLastPosition(secondPet.Id).IsSuccess;

        //assert
        moveRes.Should().BeTrue();
        firstPet.Position.Should().Be(Position.Create(1).Value);
        secondPet.Position.Should().Be(Position.Create(5).Value);
        thirdPet.Position.Should().Be(Position.Create(2).Value);
        fourthPet.Position.Should().Be(Position.Create(3).Value);
        fifthPet.Position.Should().Be(Position.Create(4).Value);
    }
    
    [Fact]
    public void Test_Move_Pet_To_First_Position_With_No_Validation_Error()
    {
        //arrange
        FullName fullName = FullName.Create("test", "test").Value;
        Email email = Email.Create("test@gmail.com").Value;
        Description description = Description.Create("test").Value;
        YearsOfExperience yearsOfExperience = YearsOfExperience.Create(12).Value;
        PhoneNumber phoneNumber = PhoneNumber.Create("89251111111").Value;
        var socialNetworks = new[]
            { SocialNetwork.Create("test", "test").Value };
        var requisites = new[]
            { Requisite.Create("test", "test").Value };

        var volunteer = Volunteer.Create(VolunteerId.NewId,
            fullName,
            email,
            description,
            yearsOfExperience,
            phoneNumber,
            socialNetworks,
            requisites);

        Name name = Name.Create("test").Value;
        PetIdentifier petIdentifier = PetIdentifier.Create(Guid.NewGuid(), Guid.NewGuid()).Value;
        PetInfo petInfo = PetInfo.Create("#fff",
            "good",
            20,
            20,
            true,
            true,
            System.DateTime.Now).Value;
        Address address = Address.Create("test", "test", "test").Value;
        PetStatus petStatus = PetStatus.LookingForHome;
        DateTime createdAt = DateTime.Now;

        volunteer.Value.AddPet(name,
            petIdentifier,
            description,
            petInfo,
            address,
            phoneNumber,
            requisites,
            petStatus,
            createdAt);
        
        volunteer.Value.AddPet(name,
            petIdentifier,
            description,
            petInfo,
            address,
            phoneNumber,
            requisites,
            petStatus,
            createdAt);
        
        volunteer.Value.AddPet(name,
            petIdentifier,
            description,
            petInfo,
            address,
            phoneNumber,
            requisites,
            petStatus,
            createdAt);
        
        volunteer.Value.AddPet(name,
            petIdentifier,
            description,
            petInfo,
            address,
            phoneNumber,
            requisites,
            petStatus,
            createdAt);
        
        volunteer.Value.AddPet(name,
            petIdentifier,
            description,
            petInfo,
            address,
            phoneNumber,
            requisites,
            petStatus,
            createdAt);

        var firstPet = volunteer.Value.Pets[0];
        var secondPet = volunteer.Value.Pets[1];
        var thirdPet = volunteer.Value.Pets[2];
        var fourthPet = volunteer.Value.Pets[3];
        var fifthPet = volunteer.Value.Pets[4];
        
        //act
        var moveRes = volunteer.Value.MovePetToFirstPosition(fifthPet.Id).IsSuccess;

        //assert
        moveRes.Should().BeTrue();
        firstPet.Position.Should().Be(Position.Create(2).Value);
        secondPet.Position.Should().Be(Position.Create(3).Value);
        thirdPet.Position.Should().Be(Position.Create(4).Value);
        fourthPet.Position.Should().Be(Position.Create(5).Value);
        fifthPet.Position.Should().Be(Position.Create(1).Value);
    }
    
    [Fact]
    public void Test_Move_Pet_To_Same_Position_With_No_Validation_Error()
    {
        //arrange
        FullName fullName = FullName.Create("test", "test").Value;
        Email email = Email.Create("test@gmail.com").Value;
        Description description = Description.Create("test").Value;
        YearsOfExperience yearsOfExperience = YearsOfExperience.Create(12).Value;
        PhoneNumber phoneNumber = PhoneNumber.Create("89251111111").Value;
        var socialNetworks = new[]
            { SocialNetwork.Create("test", "test").Value };
        var requisites = new[]
            { Requisite.Create("test", "test").Value };

        var volunteer = Volunteer.Create(VolunteerId.NewId,
            fullName,
            email,
            description,
            yearsOfExperience,
            phoneNumber,
            socialNetworks,
            requisites);

        Name name = Name.Create("test").Value;
        PetIdentifier petIdentifier = PetIdentifier.Create(Guid.NewGuid(), Guid.NewGuid()).Value;
        PetInfo petInfo = PetInfo.Create("#fff",
            "good",
            20,
            20,
            true,
            true,
            System.DateTime.Now).Value;
        Address address = Address.Create("test", "test", "test").Value;
        PetStatus petStatus = PetStatus.LookingForHome;
        DateTime createdAt = DateTime.Now;

        volunteer.Value.AddPet(name,
            petIdentifier,
            description,
            petInfo,
            address,
            phoneNumber,
            requisites,
            petStatus,
            createdAt);
        
        volunteer.Value.AddPet(name,
            petIdentifier,
            description,
            petInfo,
            address,
            phoneNumber,
            requisites,
            petStatus,
            createdAt);
        
        volunteer.Value.AddPet(name,
            petIdentifier,
            description,
            petInfo,
            address,
            phoneNumber,
            requisites,
            petStatus,
            createdAt);
        
        volunteer.Value.AddPet(name,
            petIdentifier,
            description,
            petInfo,
            address,
            phoneNumber,
            requisites,
            petStatus,
            createdAt);
        
        volunteer.Value.AddPet(name,
            petIdentifier,
            description,
            petInfo,
            address,
            phoneNumber,
            requisites,
            petStatus,
            createdAt);

        var firstPet = volunteer.Value.Pets[0];
        var secondPet = volunteer.Value.Pets[1];
        var thirdPet = volunteer.Value.Pets[2];
        var fourthPet = volunteer.Value.Pets[3];
        var fifthPet = volunteer.Value.Pets[4];
        
        //act
        var position = Position.Create(4).Value;
        
        var moveRes = volunteer.Value.MovePet(fourthPet.Id, position).IsSuccess;

        //assert
        moveRes.Should().BeTrue();
        firstPet.Position.Should().Be(Position.Create(1).Value);
        secondPet.Position.Should().Be(Position.Create(2).Value);
        thirdPet.Position.Should().Be(Position.Create(3).Value);
        fourthPet.Position.Should().Be(Position.Create(4).Value);
        fifthPet.Position.Should().Be(Position.Create(5).Value);
    }
    
    [Fact]
    public void Test_Volunteer_With_Validation_Error()
    {
        //arrange
        FullName fullName = FullName.Create("test", "test").Value;
        Description description = Description.Create("test").Value;
        YearsOfExperience yearsOfExperience = YearsOfExperience.Create(12).Value;
        PhoneNumber phoneNumber = PhoneNumber.Create("89251111111").Value;
        var socialNetworks = new[]
            { SocialNetwork.Create("test", "test").Value };
        var requisites = new[]
            { Requisite.Create("test", "test").Value };
        
        //act
        var email = Email.Create("test");

        //assert
        
        email.IsFailure.Should().BeTrue();
        email.Error.Should().Be(Errors.Volunteer.WrongEmail("test"));
    }
}