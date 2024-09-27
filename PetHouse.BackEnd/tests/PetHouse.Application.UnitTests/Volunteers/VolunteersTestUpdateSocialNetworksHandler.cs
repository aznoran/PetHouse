using CSharpFunctionalExtensions;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using PetHouse.Application.Dtos.PetManagment;
using PetHouse.Application.PetManagment;
using PetHouse.Application.PetManagment.Commands.UpdateSocialNetworks;
using PetHouse.Domain.PetManagment.Aggregate;
using PetHouse.Domain.PetManagment.ValueObjects;
using PetHouse.Domain.Shared.Id;
using PetHouse.Domain.Shared.Other;
using PetHouse.Domain.Shared.ValueObjects;

namespace PetHouse.Application.UnitTests.Volunteers;

public class VolunteersTestUpdateSocialNetworksHandler
{
    private readonly Mock<IVolunteersRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IValidator<UpdateVolunteerSocialNetworksCommand>> _validatorMock;
    private readonly Mock<ILogger<UpdateVolunteerSocialNetworksHandler>> _loggerMock;
    private readonly UpdateVolunteerSocialNetworksHandler _handler;

    public VolunteersTestUpdateSocialNetworksHandler()
    {
        _repositoryMock = new Mock<IVolunteersRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _validatorMock = new Mock<IValidator<UpdateVolunteerSocialNetworksCommand>>();
        _loggerMock = new Mock<ILogger<UpdateVolunteerSocialNetworksHandler>>();

        _handler = new UpdateVolunteerSocialNetworksHandler(
            _repositoryMock.Object,
            _loggerMock.Object,
            _validatorMock.Object,
            _unitOfWorkMock.Object
        );
    }
    
    [Fact]
    public async Task Handle_ShouldUpdateVolunteerSocialNetworks_WhenValid()
    {
        // Arrange
        var command = new UpdateVolunteerSocialNetworksCommand(
            Guid.NewGuid(),
            new List<SocialNetworksDto>
            {
                new SocialNetworksDto("https://twitter.com/volunteer", "Twitter"),
                new SocialNetworksDto("https://facebook.com/volunteer", "Facebook")
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

        var expectedSocialNetworks = command.SocialNetworksDtos
            .Select(sn => SocialNetwork.Create(sn.Link, sn.Name).Value).ToList();

        volunteer.Value.SocialNetworks[0].Name.Should().Be(expectedSocialNetworks[0].Name);
        volunteer.Value.SocialNetworks[0].Name.Should().Be(expectedSocialNetworks[0].Name);
        volunteer.Value!.SocialNetworks[1].Link.Should().Be(expectedSocialNetworks[1].Link);
        volunteer.Value!.SocialNetworks[1].Link.Should().Be(expectedSocialNetworks[1].Link);

        _unitOfWorkMock.Verify(u => u.SaveChanges(cancellationToken), Times.Once);
    }

}