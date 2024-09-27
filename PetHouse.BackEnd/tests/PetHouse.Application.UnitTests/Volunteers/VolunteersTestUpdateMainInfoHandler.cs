using CSharpFunctionalExtensions;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using PetHouse.Application.Dtos.PetManagment;
using PetHouse.Application.PetManagment;
using PetHouse.Application.PetManagment.Commands.UpdateMainInfo;
using PetHouse.Domain.PetManagment.Aggregate;
using PetHouse.Domain.PetManagment.ValueObjects;
using PetHouse.Domain.Shared;
using PetHouse.Domain.Shared.Id;
using PetHouse.Domain.Shared.Other;
using PetHouse.Domain.Shared.ValueObjects;

namespace PetHouse.Application.UnitTests.Volunteers;

public class VolunteersTestUpdateMainInfoHandler
{
    private readonly Mock<IVolunteersRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IValidator<UpdateVolunteerMainInfoCommand>> _validatorMock;
    private readonly Mock<ILogger<UpdateVolunteerMainInfoHandler>> _loggerMock;
    private readonly UpdateVolunteerMainInfoHandler _handler;

    public VolunteersTestUpdateMainInfoHandler()
    {
        _repositoryMock = new Mock<IVolunteersRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _validatorMock = new Mock<IValidator<UpdateVolunteerMainInfoCommand>>();
        _loggerMock = new Mock<ILogger<UpdateVolunteerMainInfoHandler>>();

        _handler = new UpdateVolunteerMainInfoHandler(
            _repositoryMock.Object,
            _loggerMock.Object,
            _validatorMock.Object,
            _unitOfWorkMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenVolunteerNotFound()
    {
        // Arrange
        var command = new UpdateVolunteerMainInfoCommand(
            Guid.NewGuid(),
            new UpdateVolunteerMainInfoDto(
                new FullNameDto("John", "Doe"),
                "john.doe@example.com",
                "Description",
                5,
                "89250000000"));

        var cancellationToken = CancellationToken.None;

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<UpdateVolunteerMainInfoCommand>(), cancellationToken))
            .ReturnsAsync(new ValidationResult());

        _repositoryMock.Setup(r => r.GetById(It.IsAny<Guid>(), cancellationToken))
            .ReturnsAsync(Result.Failure<Volunteer, Error>(Errors.General.NotFound(command.Id)));

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.First().Code.Should().Be(Errors.General.NotFound(command.Id).Code);
    }

    [Fact]
    public async Task Handle_ShouldUpdateVolunteerMainInfo_WhenValid()
    {
        // Arrange
        var command = new UpdateVolunteerMainInfoCommand(
            Guid.NewGuid(),
            new UpdateVolunteerMainInfoDto(
                new FullNameDto("John", "Doe"),
                "john.doe@example.com",
                "Description",
                5,
                "89653523535"));

        var cancellationToken = CancellationToken.None;

        var volunteer = Volunteer.Create(VolunteerId.NewId,
            FullName.Create("OldName", "OldSurname").Value,
            Email.Create("old.email@example.com").Value,
            Description.Create("OldDescription").Value,
            YearsOfExperience.Create(0).Value,
            PhoneNumber.Create("89587654321").Value,
            new[] { SocialNetwork.Create("test", "test").Value },
            new[] { Requisite.Create("test", "test").Value });

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<UpdateVolunteerMainInfoCommand>(), cancellationToken))
            .ReturnsAsync(new ValidationResult());

        _repositoryMock.Setup(r => r.GetById(It.IsAny<Guid>(), cancellationToken))
            .ReturnsAsync(volunteer);
        
        // Act
        var result = await _handler.Handle(command, cancellationToken);


        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(command.Id, result.Value);

        _unitOfWorkMock.Verify(u => u.SaveChanges(cancellationToken), Times.Once);

        Assert.Equal("John", volunteer.Value.FullName.Name);
        Assert.Equal("Doe", volunteer.Value.FullName.Surname);
        Assert.Equal("john.doe@example.com", volunteer.Value.Email.Value);
        Assert.Equal("Description", volunteer.Value.Description.Value);
        Assert.Equal(5, volunteer.Value.YearsOfExperience.Value);
        Assert.Equal("89653523535", volunteer.Value.PhoneNumber.Value);
        Assert.NotEqual("89587654321", volunteer.Value.PhoneNumber.Value);
    }
}