using CSharpFunctionalExtensions;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using PetHouse.Application.Dtos.PetManagment;
using PetHouse.Application.PetManagement;
using PetHouse.Application.PetManagement.Commands.Create;
using PetHouse.Domain.PetManagment.Aggregate;
using PetHouse.Domain.PetManagment.ValueObjects;
using PetHouse.Domain.Shared;
using PetHouse.Domain.Shared.Other;
using PetHouse.Domain.Shared.ValueObjects;

namespace PetHouse.Application.UnitTests.Volunteers;

public class VolunteersTestCreateHandler
{
    private readonly Mock<IVolunteersRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IValidator<CreateVolunteerCommand>> _validatorMock;
    private readonly Mock<ILogger<CreateVolunteerHandler>> _loggerMock;
    private readonly CreateVolunteerHandler _handler;

    public VolunteersTestCreateHandler()
    {
        _repositoryMock = new Mock<IVolunteersRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _validatorMock = new Mock<IValidator<CreateVolunteerCommand>>();
        _loggerMock = new Mock<ILogger<CreateVolunteerHandler>>();
        
        _handler = new CreateVolunteerHandler(
            _repositoryMock.Object,
            _loggerMock.Object,
            _validatorMock.Object,
            _unitOfWorkMock.Object
            );
    }

    [Fact]
    public async Task Handle_ShouldSucceed_WhenAllConditionsAreMet()
    {
        // Arrange
        var command = new CreateVolunteerCommand(
            new FullNameDto("John", "Doe"),
            "Experienced volunteer",
            "test@gmail.com",
            3,
            "82345678906",
            new List<SocialNetworksDto>
            {
                new SocialNetworksDto("test", "test")
            },
            new List<RequisiteDto>
            {
                new RequisiteDto("test", "test")
            });
    
        var cancellationToken = CancellationToken.None;

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<CreateVolunteerCommand>(), cancellationToken))
            .ReturnsAsync(new ValidationResult());

        _repositoryMock.Setup(r => r.GetByEmail(It.IsAny<Email>(), cancellationToken))
            .ReturnsAsync(Result.Failure<Volunteer, Error>(Error.NotFound("record.not.found", "Not found")));

        _repositoryMock.Setup(r => r.GetByPhoneNumber(It.IsAny<PhoneNumber>(), cancellationToken))
            .ReturnsAsync(Result.Failure<Volunteer,Error>(Error.NotFound("record.not.found", "Not found")));

        _repositoryMock.Setup(r => r.Create(It.IsAny<Volunteer>(), cancellationToken))
            .ReturnsAsync(Guid.NewGuid());

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEqual(result.Value, Guid.Empty);
        _repositoryMock.Verify(r => r.GetByEmail(It.IsAny<Email>(), cancellationToken), Times.Once);
        _repositoryMock.Verify(r => r.GetByPhoneNumber(It.IsAny<PhoneNumber>(), cancellationToken), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChanges(cancellationToken), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmailExists_WhenVolunteerCreated()
    {
        // Arrange
        var command = new CreateVolunteerCommand(
            new FullNameDto("John", "Doe"),
            "Experienced volunteer",
            "test@gmail.com",
            3,
            "82345678950",
            new List<SocialNetworksDto>
            {
                new SocialNetworksDto("test", "test")
            },
            new List<RequisiteDto>
            {
                new RequisiteDto("test", "test")
            });
    
        var cancellationToken = CancellationToken.None;

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<CreateVolunteerCommand>(), cancellationToken))
            .ReturnsAsync(new ValidationResult());

        _repositoryMock.Setup(r => r.GetByEmail(It.IsAny<Email>(), cancellationToken))
            .ReturnsAsync(Result.Failure<Volunteer,Error>(Errors.General.NotFound()));

        // Act
        var res = await _handler.Handle(command, cancellationToken);

        // Assert
        res.Error.First().Code.Should().Be("already.exists");
    }

}