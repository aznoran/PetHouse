using CSharpFunctionalExtensions;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using PetHouse.Application.Volunteers;
using PetHouse.Application.Volunteers.Commands.Delete;
using PetHouse.Domain.PetManagment.Aggregate;
using PetHouse.Domain.Shared;
using PetHouse.Domain.Shared.Other;
using PetHouse.Domain.Shared.ValueObjects;

namespace PetHouse.Application.UnitTests.Volunteers;

public class VolunteersTestDeleteHandler
{
    private readonly Mock<IVolunteersRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IValidator<DeleteVolunteerCommand>> _validatorMock;
    private readonly Mock<ILogger<DeleteVolunteerHandler>> _loggerMock;
    private readonly DeleteVolunteerHandler _handler;

    public VolunteersTestDeleteHandler()
    {
        _repositoryMock = new Mock<IVolunteersRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _validatorMock = new Mock<IValidator<DeleteVolunteerCommand>>();
        _loggerMock = new Mock<ILogger<DeleteVolunteerHandler>>();
        
        _handler = new DeleteVolunteerHandler(
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
        var command = new DeleteVolunteerCommand(Guid.NewGuid());
        var cancellationToken = CancellationToken.None;

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<DeleteVolunteerCommand>(), cancellationToken))
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
    public async Task Handle_ShouldDeleteVolunteer_WhenFound()
    {
        // Arrange
        var command = new DeleteVolunteerCommand(Guid.NewGuid());
        var cancellationToken = CancellationToken.None;
        var volunteer = new Volunteer();

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<DeleteVolunteerCommand>(), cancellationToken))
            .ReturnsAsync(new ValidationResult());

        _repositoryMock.Setup(r => r.GetById(It.IsAny<Guid>(), cancellationToken))
            .ReturnsAsync(volunteer);

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(command.Id);
        _unitOfWorkMock.Verify(u => u.SaveChanges(cancellationToken), Times.Once);
    }

}