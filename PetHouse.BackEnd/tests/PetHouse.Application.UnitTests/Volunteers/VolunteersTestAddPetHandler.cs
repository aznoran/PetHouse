using CSharpFunctionalExtensions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using PetHouse.Application.Dto;
using PetHouse.Application.Volunteers;
using PetHouse.Application.Volunteers.AddPet;
using PetHouse.Domain.Enums;
using PetHouse.Domain.Models;
using PetHouse.Domain.Models.Other;
using PetHouse.Domain.Models.Volunteers.ValueObjects;
using PetHouse.Domain.Shared;
using PetHouse.Domain.ValueObjects;
using PetHouse.Infrastructure;

namespace PetHouse.Application.UnitTests.Volunteers;

public class VolunteersTestAddPetHandler
{
    private readonly Mock<IVolunteersRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IValidator<AddPetCommand>> _validatorMock;
    private readonly Mock<ILogger<AddPetHandler>> _loggerMock;
    private readonly AddPetHandler _handler;

    public VolunteersTestAddPetHandler()
    {
        _repositoryMock = new Mock<IVolunteersRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _validatorMock = new Mock<IValidator<AddPetCommand>>();
        _loggerMock = new Mock<ILogger<AddPetHandler>>();
        
        _handler = new AddPetHandler(
            _repositoryMock.Object,
            _loggerMock.Object,
            _unitOfWorkMock.Object,
            _validatorMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnValidationErrors_WhenValidationFails()
    {
        // Arrange
        var command = new AddPetCommand(Guid.Empty, null);
        var cancellationToken = CancellationToken.None;

        var validationResult = new ValidationResult(new List<ValidationFailure>
        {
            new ValidationFailure("PropertyName", "Error message|Validation Error|Validation")
        });

        _validatorMock.Setup(v => v.ValidateAsync(command, cancellationToken))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Validation Error", result.Error.First().Message);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFoundError_WhenVolunteerNotFound()
    {
        // Arrange
        var command = new AddPetCommand(Guid.NewGuid(), null);
        var cancellationToken = CancellationToken.None;

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<AddPetCommand>(), cancellationToken))
            .ReturnsAsync(new ValidationResult());

        _repositoryMock.Setup(r => r.GetById(command.VolunteerId, cancellationToken))
            .ReturnsAsync(Errors.General.NotFound());

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("record.not.found", result.Error.First().Code);
    }

    [Fact]
    public async Task Handle_ShouldSucceed_WhenAllConditionsAreMet()
    {
        // Arrange
        var addPetDto = new AddPetDto(
            "Buddy",
            new PetIdentifierDto
                (Guid.NewGuid(), Guid.NewGuid()),
            "Friendly dog",
            "#fff",
            "Good",
            15,
            40,
            true,
            true,
            DateTime.UtcNow.AddYears(-2),
            "Test City",
            "Test Street",
            "Test Country",
            "82345678901",
            new List<RequisiteDto>
            {
                new RequisiteDto("Requisite 1", "Desc 1")
            },
            PetStatus.LookingForHome);


        var command = new AddPetCommand(Guid.NewGuid(), addPetDto);

        var cancellationToken = CancellationToken.None;

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<AddPetCommand>(), cancellationToken))
            .ReturnsAsync(new ValidationResult());

        FullName fullName = FullName.Create("test", "test").Value;
        Email email = Email.Create("test@gmail.com").Value;
        Description description = Description.Create("test").Value;
        YearsOfExperience yearsOfExperience = YearsOfExperience.Create(12).Value;
        PhoneNumber phoneNumber = PhoneNumber.Create("89251111111").Value;
        SocialNetworkInfo? socialNetworks = new SocialNetworkInfo(new[]
            { SocialNetwork.Create("test", "test").Value });
        RequisiteInfo? requisites = new RequisiteInfo(new[]
            { Requisite.Create("test", "test").Value });
        
        var volunteer = Volunteer.Create(VolunteerId.Create(command.VolunteerId),
            fullName,
            email,
            description,
            yearsOfExperience,
            phoneNumber,
            socialNetworks,
            requisites).Value;
        
        _repositoryMock.Setup(r => r.GetById(command.VolunteerId, cancellationToken))
            .ReturnsAsync(Result.Success<Volunteer, Error>(volunteer));

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        _unitOfWorkMock.Verify(u => u.SaveChanges(cancellationToken), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldCallUnitOfWork_WhenVolunteerExists()
    {
        // Arrange
        var addPetDto = new AddPetDto(
            "Buddy",
            new PetIdentifierDto
                (Guid.NewGuid(), Guid.NewGuid()),
            "Friendly dog",
            "#fff",
            "Good",
            15,
            40,
            true,
            true,
            DateTime.UtcNow.AddYears(-2),
            "Test City",
            "Test Street",
            "Test Country",
            "82345678901",
            new List<RequisiteDto>
            {
                new RequisiteDto("Requisite 1", "Desc 1")
            },
            PetStatus.LookingForHome);


        var command = new AddPetCommand(Guid.NewGuid(), addPetDto);
        var cancellationToken = CancellationToken.None;

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<AddPetCommand>(), cancellationToken))
            .ReturnsAsync(new ValidationResult());

        FullName fullName = FullName.Create("test", "test").Value;
        Email email = Email.Create("test@gmail.com").Value;
        Description description = Description.Create("test").Value;
        YearsOfExperience yearsOfExperience = YearsOfExperience.Create(12).Value;
        PhoneNumber phoneNumber = PhoneNumber.Create("89251111111").Value;
        SocialNetworkInfo? socialNetworks = new SocialNetworkInfo(new[]
            { SocialNetwork.Create("test", "test").Value });
        RequisiteInfo? requisites = new RequisiteInfo(new[]
            { Requisite.Create("test", "test").Value });
        
        var volunteer = Volunteer.Create(VolunteerId.Create(command.VolunteerId),
            fullName,
            email,
            description,
            yearsOfExperience,
            phoneNumber,
            socialNetworks,
            requisites).Value;
        
        _repositoryMock.Setup(r => r.GetById(command.VolunteerId, cancellationToken))
            .ReturnsAsync(Result.Success<Volunteer, Error>(volunteer));

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        _unitOfWorkMock.Verify(u => u.SaveChanges(cancellationToken), Times.Once);
        _repositoryMock.Verify(r => r.GetById(command.VolunteerId, cancellationToken), Times.Once);
    }
}