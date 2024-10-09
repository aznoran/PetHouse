using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using PetHouse.Core.Dtos.PetManagment;
using PetHouse.PetManagement.Application;
using PetHouse.PetManagement.Application.Commands.UpdateRequisites;
using PetHouse.PetManagement.Domain.Aggregate;
using PetHouse.PetManagement.Domain.ValueObjects;
using PetHouse.SharedKernel.Id;
using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.Application.UnitTests.Volunteers;

public class VolunteersTestUpdateRequisitesHandler
{
    private readonly Mock<IVolunteersRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IValidator<UpdateVolunteerRequisitesCommand>> _validatorMock;
    private readonly Mock<ILogger<UpdateVolunteerRequisitesHandler>> _loggerMock;
    private readonly UpdateVolunteerRequisitesHandler _handler;

    public VolunteersTestUpdateRequisitesHandler()
    {
        _repositoryMock = new Mock<IVolunteersRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _validatorMock = new Mock<IValidator<UpdateVolunteerRequisitesCommand>>();
        _loggerMock = new Mock<ILogger<UpdateVolunteerRequisitesHandler>>();

        _handler = new UpdateVolunteerRequisitesHandler(
            _repositoryMock.Object,
            _loggerMock.Object,
            _validatorMock.Object,
            _unitOfWorkMock.Object
        );
    }
    
    [Fact]
    public async Task Handle_ShouldUpdateVolunteerRequisites_WhenValid()
    {
        // Arrange
        var command = new UpdateVolunteerRequisitesCommand(
            Guid.NewGuid(),
            new List<RequisiteDto>
            {
                new RequisiteDto("Bank Account", "12345"),
                new RequisiteDto("PayPal", "paypal@example.com")
            });

        var cancellationToken = CancellationToken.None;

        var volunteer = Volunteer.Create(VolunteerId.NewId,
            FullName.Create("OldName", "OldSurname").Value,
            Email.Create("old.email@example.com").Value,
            Description.Create("OldDescription").Value,
            YearsOfExperience.Create(0).Value,
            PhoneNumber.Create("89587654321").Value,
            new[] { SocialNetwork.Create("test", "test").Value },
            new[] { Requisite.Create("test", "test").Value });

        _validatorMock.Setup(v => v.ValidateAsync(command, cancellationToken))
            .ReturnsAsync(new ValidationResult());

        _repositoryMock.Setup(r => r.GetById(command.Id, cancellationToken))
            .ReturnsAsync(volunteer);

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(command.Id, result.Value);

        var expectedRequisites = command.RequisiteDto
            .Select(r => Requisite.Create(r.Name, r.Description).Value).ToList();
        volunteer.Value.Requisites[0].Name.Should().Be(expectedRequisites[0].Name);
        volunteer.Value.Requisites[0].Name.Should().Be(expectedRequisites[0].Name);
        volunteer.Value.Requisites[1].Description.Should().Be(expectedRequisites[1].Description);
        volunteer.Value.Requisites[1].Description.Should().Be(expectedRequisites[1].Description);

        _unitOfWorkMock.Verify(u => u.SaveChanges(cancellationToken), Times.Once);
    }

}