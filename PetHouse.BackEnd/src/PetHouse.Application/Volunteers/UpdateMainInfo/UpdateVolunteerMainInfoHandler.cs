using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetHouse.Domain.Models.Volunteers.ValueObjects;
using PetHouse.Domain.Shared;

namespace PetHouse.Application.Volunteers.UpdateMainInfo;

public class UpdateVolunteerMainInfoHandler : IUpdateVolunteerMainInfoHandler
{
    private readonly IVolunteersRepository _repository;
    private readonly ILogger<UpdateVolunteerMainInfoHandler> _logger;

    public UpdateVolunteerMainInfoHandler(
        IVolunteersRepository repository,
        ILogger<UpdateVolunteerMainInfoHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    
    public async Task<Result<Guid, Error>> Handle(
        UpdateVolunteerMainInfoRequest request,
        CancellationToken cancellationToken)
    {
        var volunteerResult = await _repository.GetById(request.Id, cancellationToken);

        if (volunteerResult.IsFailure)
        {
            return Errors.General.NotFound(request.Id);
        }
        
        var fullName = FullName.Create(request.UpdateVolunteerMainInfoDto.FullNameDto.Name, request.UpdateVolunteerMainInfoDto.FullNameDto.Surname).Value;

        var email = Email.Create(request.UpdateVolunteerMainInfoDto.Email).Value;
        
        var description = Description.Create(request.UpdateVolunteerMainInfoDto.Description).Value;

        var yearsOfExperience = YearsOfExperience.Create(request.UpdateVolunteerMainInfoDto.YearsOfExperience).Value;

        var phoneNumber = PhoneNumber.Create(request.UpdateVolunteerMainInfoDto.PhoneNumber).Value;

        volunteerResult.Value.UpdateMainInfo(
            fullName,
            email,
            description,
            yearsOfExperience,
            phoneNumber);

        await _repository.Save(volunteerResult.Value, cancellationToken);

        _logger.LogInformation("Updated {Volunteer} with id {Id}", volunteerResult.Value, request.Id);
        
        return request.Id;
    }
}